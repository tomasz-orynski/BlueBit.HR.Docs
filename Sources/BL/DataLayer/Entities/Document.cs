namespace BlueBit.HR.Docs.BL.DataLayer.Entities
{
    public interface IDocument :
        Commons.IObjectInDBWithID
    {
        string PESEL { get; }
        short DateYear { get; }
        byte DateMonth { get; }
        IDocumentsLoad DocumentsLoad { get; }
    }

    public interface IDocumentWithData :
        IDocument
    {
        byte[] Data { get; }
    }

    public class DocumentBase :
        IDocument
    {
        public virtual long ID { get; set; }

        public virtual string PESEL { get; set; }
        public virtual short DateYear { get; set; }
        public virtual byte DateMonth { get; set; }
        public virtual DocumentsLoad DocumentsLoad { get; set; }
        IDocumentsLoad IDocument.DocumentsLoad { get { return DocumentsLoad; } }
    }

    public class DocumentWithData :
        DocumentBase,
        IDocumentWithData
    {
        public virtual byte[] Data { get; set; }
    }

    public class DocumentWithoutDataAndLastVer :
        DocumentBase
    {
    }
}
