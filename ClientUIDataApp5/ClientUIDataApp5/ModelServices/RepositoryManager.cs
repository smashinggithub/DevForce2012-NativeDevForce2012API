using ClientUIDataApp5.DomainModel;

namespace ClientUIDataApp5.ModelServices
{
    public class RepositoryManager
    {
        public static NorthwindEntities Create()
        {
            return new NorthwindEntities();
        }
    }
}
