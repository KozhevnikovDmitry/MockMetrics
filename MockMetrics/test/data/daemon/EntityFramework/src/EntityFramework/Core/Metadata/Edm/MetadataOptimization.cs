﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Metadata.Edm
{
    using System.Collections.Generic;
    using System.Data.Entity.Utilities;
    using System.Diagnostics;
    using System.Threading;

    internal class MetadataOptimization
    {
        private readonly MetadataWorkspace _workspace;

        private volatile AssociationType[] _csAssociationTypes;
        private volatile AssociationType[] _osAssociationTypes;
        private volatile object[] _csAssociationTypeToSets;

        internal MetadataOptimization(MetadataWorkspace workspace)
        {
            DebugCheck.NotNull(workspace);

            _workspace = workspace;
        }

        #region CSpace

        internal AssociationType GetCSpaceAssociationType(AssociationType osAssociationType)
        {
            Debug.Assert(osAssociationType.Index >= 0);

            return _csAssociationTypes[osAssociationType.Index];
        }

        internal AssociationSet FindCSpaceAssociationSet(AssociationType csAsociationType, Predicate<AssociationSet> predicate)
        {
            return GetItemAtIndex(GetCSpaceAssociationTypeToSetsMap(), csAsociationType.Index, predicate);
        }

        // Internal for testing only.
        internal AssociationType[] GetCSpaceAssociationTypes()
        {
            if (_csAssociationTypes == null)
            {
                _csAssociationTypes = IndexCSpaceAssociationTypes(_workspace.GetItemCollection(DataSpace.CSpace));
            }

            return _csAssociationTypes;
        }

        private static AssociationType[] IndexCSpaceAssociationTypes(ItemCollection itemCollection)
        {
            Debug.Assert(itemCollection.DataSpace == DataSpace.CSpace);
            Debug.Assert(itemCollection.IsReadOnly);

            var associationTypes = new List<AssociationType>();
            var count = 0;

            foreach (var associatonType in itemCollection.GetItems<AssociationType>())
            {
                associationTypes.Add(associatonType);
                associatonType.Index = count++;
            }

            return associationTypes.ToArray();
        }

        // Internal for testing only.
        internal object[] GetCSpaceAssociationTypeToSetsMap()
        {
            if (_csAssociationTypeToSets == null)
            {
                _csAssociationTypeToSets = MapCSpaceAssociationTypeToSets(
                    _workspace.GetItemCollection(DataSpace.CSpace), GetCSpaceAssociationTypes().Length);
            }

            return _csAssociationTypeToSets;
        }

        private static object[] MapCSpaceAssociationTypeToSets(ItemCollection itemCollection, int associationTypeCount)
        {
            Debug.Assert(itemCollection.DataSpace == DataSpace.CSpace);
            Debug.Assert(itemCollection.IsReadOnly);

            var associationTypeToSets = new object[associationTypeCount];

            foreach (var entityContainer in itemCollection.GetItems<EntityContainer>())
            {
                foreach (var baseEntitySet in entityContainer.BaseEntitySets)
                {
                    var associationSet = baseEntitySet as AssociationSet;
                    if (associationSet != null)
                    {
                        var j = associationSet.ElementType.Index;
                        Debug.Assert(j >= 0);

                        AddItemAtIndex(associationTypeToSets, j, associationSet);
                    }
                }
            }

            return associationTypeToSets;
        }

        #endregion

        #region OSpace

        internal AssociationType GetOSpaceAssociationType(
            AssociationType cSpaceAssociationType, Func<AssociationType> initializer)
        {
            Debug.Assert(cSpaceAssociationType.DataSpace == DataSpace.CSpace);

            var oSpaceAssociationTypes = GetOSpaceAssociationTypes();
            var index = cSpaceAssociationType.Index;

            Thread.MemoryBarrier(); 
            var oSpaceAssociationType = oSpaceAssociationTypes[index];

            if (oSpaceAssociationType == null)
            {
                oSpaceAssociationType = initializer();
                Debug.Assert(oSpaceAssociationType.DataSpace == DataSpace.OSpace);

                oSpaceAssociationType.Index = index;
                oSpaceAssociationTypes[index] = oSpaceAssociationType;
                Thread.MemoryBarrier(); 
            }

            return oSpaceAssociationType;
        }

        // Internal for testing only.
        internal AssociationType[] GetOSpaceAssociationTypes()
        {
            if (_osAssociationTypes == null)
            {
                _osAssociationTypes = new AssociationType[GetCSpaceAssociationTypes().Length];
            }

            return _osAssociationTypes;
        }

        #endregion

        #region Helper methods

        private static void AddItemAtIndex<T>(object[] array, int index, T newItem)
            where T : class
        {
            var objectAtIndex = array[index];
            if (objectAtIndex == null)
            {
                array[index] = newItem;
                return;
            }

            var item = objectAtIndex as T;
            if (item != null)
            {
                array[index] = new[] { item, newItem };
                return;
            }

            var items = (T[])objectAtIndex;
            var count = items.Length;
            Array.Resize(ref items, count + 1);
            items[count] = newItem;
            array[index] = items;
        }

        private static T GetItemAtIndex<T>(object[] array, int index, Predicate<T> predicate)
            where T : class
        {
            var objectAtIndex = array[index];
            if (objectAtIndex == null)
            {
                return null;
            }

            var item = objectAtIndex as T;
            if (item != null)
            {
                return predicate(item) ? item : null;
            }

            var items = (T[])objectAtIndex;
            for (var i = 0; i < items.Length; i++)
            {
                item = items[i];
                if (predicate(item))
                {
                    return item;
                }
            }

            return null;
        }

        #endregion
    }
}
