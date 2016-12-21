using System;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace BlueBit.HR.Docs.BL.BusinessLayer.Services.DataLoad
{
    public interface ILoadZipServiceContext : IServiceContext
    {
        Ionic.Zip.ZipFile GetZipFile();
    }

    public class LoadZipServiceContext : 
        ServiceContext,
        ILoadZipServiceContext
    {
        private readonly Func<Ionic.Zip.ZipFile> zipFileAction;
        public Ionic.Zip.ZipFile GetZipFile() { return zipFileAction(); } 

        public LoadZipServiceContext(IBusinessTransaction businessTransaction, string zipPath) : base(businessTransaction)
        {
            Contract.Requires(!string.IsNullOrEmpty(zipPath));
            zipFileAction = () => Ionic.Zip.ZipFile.Read(zipPath);
        }
        public LoadZipServiceContext(IBusinessTransaction businessTransaction, System.IO.Stream zipStream)
            : base(businessTransaction)
        {
            Contract.Requires(zipStream != null);
            zipFileAction = () => Ionic.Zip.ZipFile.Read(zipStream);
        }
    }

    public class LoadZipServiceException : ServiceException
    {
        static public void Throw(Exception e) { throw new LoadZipServiceException(); }
        static public void Throw(string zipEntryName, Exception e) { throw new LoadZipServiceException(); }
    }

    public sealed class LoadZipService<TServiceContext> : ServiceBase<TServiceContext, ulong>
        where TServiceContext : class, ILoadZipServiceContext
    {
        public LoadZipService(TServiceContext context) : base(context) { }

        protected override ulong OnExecute()
        {
            var cnt = 0UL;
            try
            {
                var dbSession = ServiceCtx.BusinessTransaction.DBSession;

                var documentsLoad = new DataLayer.Entities.DocumentsLoad();
                documentsLoad.DateStart = DateTime.Now;
                dbSession.Insert(documentsLoad);

                foreach (var document in GetDocuments())
                {
                    document.DocumentsLoad = documentsLoad;
                    dbSession.Insert(document);
                    ++cnt;
                }

                documentsLoad.DateStop = DateTime.Now;
                dbSession.Update(documentsLoad);
            }
            catch (Exception e)
            {
                LoadZipServiceException.Throw(e);
            }
            return cnt;
        }

        private IEnumerable<DataLayer.Entities.DocumentWithData> GetDocuments()
        {
            using (var zipFile = ServiceCtx.GetZipFile())
            {
                foreach (var zipEntry in zipFile.EntriesSorted.Where(e => !e.IsDirectory).ToArray())
                    yield return GetDocument(zipEntry);
            }
        }

        private static DataLayer.Entities.DocumentWithData GetDocument(Ionic.Zip.ZipEntry zipEntry)
        {
            try
            {
                var zipEntryNameParts = zipEntry.FileName.Split('/');
                if (zipEntryNameParts.Length != 3) throw new IndexOutOfRangeException();
                var year = Convert.ToInt16(zipEntryNameParts[0]);
                var month = Convert.ToByte(zipEntryNameParts[1]);
                var PESEL = System.IO.Path.GetFileNameWithoutExtension(zipEntryNameParts[2]);

                return new DataLayer.Entities.DocumentWithData()
                {
                    DateYear = year,
                    DateMonth = month,
                    PESEL = PESEL,
                    Data = GetDocumentData(zipEntry),
                };
            }
            catch (Exception e)
            {
                LoadZipServiceException.Throw(zipEntry.FileName, e);
                return null; //TODO
            }
        }

        private static byte[] GetDocumentData(Ionic.Zip.ZipEntry zipEntry)
        {
            using (var zipReader = zipEntry.OpenReader())
            {
                var buffer = new byte[zipReader.Length];
                zipReader.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
    }
}
