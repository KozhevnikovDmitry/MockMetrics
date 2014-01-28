using System.Collections.Generic;
using System.Linq;
using BLToolkit.Common;
using Common.BL.Validation;
using Common.UI.ViewModel.ListViewModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.UI.ViewModel.ClaimViewModelViewModel
{
    public class ClaimCategoryItemVM : AbstractListItemVM<ClaimCategory>
    {
        public ClaimCategoryItemVM(ClaimCategory entity, IDomainValidator<ClaimCategory> domainValidator, bool isValidateable)
            : base(entity, domainValidator, isValidateable)
        {
            if (ClaimCategoryList == null)
            {
                var temp = HqFacade.GetDictionaryManager().GetDictionary<CategoryType>().Select(categoryType => categoryType.Clone()).ToList();
                ClaimCategoryList = getCategoryTreeFromList(temp);
            }
        }

      
        protected override void Initialize()
        {
        }
        
        /// <summary>
        /// Список оснований категорий
        /// </summary>
        public List<CategoryType> ClaimCategoryList { get; private set; }
       
        /// <summary>
        /// ID  категории
        /// </summary>
        public int CategoryTypeId
        {
            get { return Entity.CategoryTypeId; }
            set
            {
                if (Entity != null && Entity.CategoryTypeId != value)
                    Entity.CategoryTypeId = value;
            }
        }

        /// <summary>
        /// Причесывание листа категорий под вид дерева
        /// Этот метод символизирует собой страшное костылестроние и является позором этого проекта, но он работает.
        /// </summary>
        /// <param name="CategoryList"></param>
        /// <returns></returns>
        private List<CategoryType> getCategoryTreeFromList(List<CategoryType> CategoryList)
        {
            var categoryListCur = new List<CategoryType>();
            var categoryListCur2 = new List<CategoryType>();
            
            int cntSpace = 1; //уровень вложенности списка

            string str = null;

            // забивка корневого уровня "дерева" элементами не имеющими родителей
            for (int i = 0; i < CategoryList.Count; i++)
            {
                if (CategoryList[i].ParentCategoryId == null)
                    categoryListCur.Add(CategoryList[i]);
            }

            bool fl = true;

            //пока список не перестанет расти будем гонять по циклу
            while (fl)
            {
                int sCnt = categoryListCur.Count; // начальный размер списка 
                int eCnt = 0;                     // результирующий размер списка

                foreach (CategoryType itemCategiryListCur in categoryListCur)
                {
                    categoryListCur2.Add(itemCategiryListCur);
                    int curRoot = categoryListCur2.Count - 1; //текущий корневой узел
                    foreach (CategoryType itemCategoryList in CategoryList)
                        if (itemCategoryList.ParentCategoryId != null
                            && itemCategoryList.ParentCategoryId.Equals(categoryListCur2[curRoot].Id.ToString())  // элемент списка является потомком  
                            && !categoryListCur2.Contains(itemCategoryList)                                       // не содержится в списке
                            && !categoryListCur.Contains(itemCategoryList))
                        {
                            str = "";
                            categoryListCur2.Add(itemCategoryList);
                            for (var k = 0; k < cntSpace; k++)
                                str += "   ";
                            categoryListCur2[categoryListCur2.Count - 1].Name = str + categoryListCur2[categoryListCur2.Count - 1].Name; // делаем отступы
                        }
                }
                cntSpace++;
                categoryListCur = categoryListCur2;
                categoryListCur2 = new List<CategoryType>();
                eCnt = categoryListCur.Count;
                if (eCnt == sCnt)
                    fl = false;
            }
            return categoryListCur;
        }

    }
}
