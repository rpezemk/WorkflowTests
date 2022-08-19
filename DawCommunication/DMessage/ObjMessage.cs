namespace DawCommunication
{
    public class ObjMessage<T> : AMessage<T> where T: new()
    {
        public ObjMessage() { }
        public ObjMessage(string info, string type = "") : base(typeof(T).GetType().Name, info) { }


        public override T GetObject()
        {
            return base.TObject;
        }

        public override TxtMessage GetTextMessage() 
        {
            Serializer serializer = new Serializer();
            var testClass = new T();
            var xmlText = serializer.Serialize(testClass);
            TxtMessage txtMessage = new TxtMessage();
            txtMessage.Data = xmlText;
            return txtMessage;
        }
    }

}
