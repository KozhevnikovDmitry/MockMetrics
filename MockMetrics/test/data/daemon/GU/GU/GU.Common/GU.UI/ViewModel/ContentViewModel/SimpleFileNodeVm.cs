using System;
using System.IO;
using System.Linq;

using Common.BL.Validation;
using Common.UI;

using GU.DataModel;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;

namespace GU.UI.ViewModel.ContentViewModel
{
    public class SimpleFileNodeVm : SimpleNodeVm
    {
        public SimpleFileNodeVm(ContentNode entity, IDomainValidator<ContentNode> domainValidator, ContentNodeVmBuilder contentNodeVmBuilder, bool isValidateable = true)
            : base(entity, domainValidator, contentNodeVmBuilder, isValidateable)
        {
            this.LoadFileFromDiskCommand = new DelegateCommand(this.LoadFileFromDisk);
            this.SaveFileOnDiskCommand = new DelegateCommand(this.SaveFileOnDick, this.CanSaveFileonDisk);
        }

        #region Binding Properties

        public string FileName
        {
            get
            {
                return this.Entity.BlobName;
            }
            private set
            {
                if (this.Entity.BlobName != value)
                {
                    this.Entity.BlobName = value;
                    this.RaisePropertyChanged(() => this.FileName);
                }
            }
        }

        public int BlobSize
        {
            get
            {
                return this.Entity.BlobSize;
            }
            private set
            {
                if (this.Entity.BlobSize != value)
                {
                    this.Entity.BlobSize = value;
                    this.RaisePropertyChanged(() => this.BlobSize);
                }
            }
        }

        public string BlobType
        {
            get
            {
                return this.Entity.BlobType;
            }
            private set
            {
                if (this.Entity.BlobType != value)
                {
                    this.Entity.BlobType = value;
                    this.RaisePropertyChanged(() => this.BlobType);
                }
            }
        }

        public byte[] BlobValue
        {
            get
            {
                return this.Entity.BlobValue;
            }
            private set
            {
                if (this.Entity.BlobValue != value)
                {
                    this.Entity.BlobValue = value;
                    this.RaisePropertyChanged(() => this.FileName);
                }
            }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand SaveFileOnDiskCommand { get; protected set; }

        private void SaveFileOnDick()
        {
            try
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = string.Format("Все файлы (*.*)|*.*");
                saveFileDialog.FileName = this.FileName;
                if (saveFileDialog.ShowDialog() == true)
                {
                    if (!File.Exists(saveFileDialog.FileName))
                    {
                        File.WriteAllBytes(saveFileDialog.FileName, BlobValue);
                    }
                }
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
        }

        private bool CanSaveFileonDisk()
        {
            return BlobValue != null && !string.IsNullOrEmpty(FileName);
        }

        public DelegateCommand LoadFileFromDiskCommand { get; protected set; }

        private void LoadFileFromDisk()
        {
            try
            {
                var ofd = new OpenFileDialog();
                ofd.Filter = string.Format("Все файлы {0}| Документы Word  {1}| Документы Excel  {2}| Документы Acrobat  {3}| Файлы изображений  {4}",
                    "(*.*)|*.*",
                    "(*.jpg, *.jpeg, *.png, *.bmp)|*.jpg; *.jpeg; *.png; *.bmp",
                    "(*.doc, *.docx)|*.doc; *.docx",
                    "(*.xls, *.xlsx)|*.xls; *.xlsx",
                    "(*.pdf)|*.pdf; *.PDF");
                if (ofd.ShowDialog() == true)
                {
                    this.FileName = Path.GetFileName(ofd.FileName);
                    this.BlobValue = this.StreamFile(ofd.FileName);
                    this.BlobSize = this.BlobValue.Count();
                    this.RaisePropertyChanged(() => this.FileName);
                    SaveFileOnDiskCommand.RaiseCanExecuteChanged();
                }
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
        }

        private byte[] StreamFile(string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, System.Convert.ToInt32(fs.Length));
                fs.Close();
                return data;
            }
        }

        #endregion

        public override void RaiseValidatingPropertyChanged()
        {
            this.RaisePropertyChanged(() => FileName);
        }
    }
}