using HashidsNet;

namespace WhatsYourIdea.Applications.Hasher
{
    public class HasherService
    {
        private readonly string _salt;

        public HasherService(HasherSetting setting)
        {
            _salt = setting.Salt;
        }

        public string Encode(int toEncode)
        {
            var hashids = new Hashids(_salt);
            return hashids.Encode(toEncode);
        }

        public int Decode(string toDecode)
        {
            var hashids = new Hashids(_salt);
            return hashids.DecodeSingle(toDecode);
        }
    }
}