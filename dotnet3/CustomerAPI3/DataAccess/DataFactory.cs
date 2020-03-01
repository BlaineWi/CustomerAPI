using System.Collections.Generic;
using System.Linq;

namespace CustomerAPI3.DataAccess
{
    /// <summary>
    /// Data Factory
    /// </summary>
    public static class DataFactory
    {
        private const int PeopleCount = 100;

        private static List<Models.Customer> _list = null;

        /// <summary>
        /// Person List
        /// </summary>
        public static List<Models.Customer> PersonList
        {
            get
            {
                if (_list == null)
                {
                    _list = new List<Models.Customer>();
                    for (int i = 0; i < PeopleCount; i++)
                    {
                        var p = ModelMaker.PersonMake();
                        _list.Add(p);
                    }
                }
                return _list;
            }
            set
            {
                _list = value;
            }
        }

        /// <summary>
        /// People
        /// </summary>
        public static IQueryable<Models.Customer> People
        {
            get
            {
                return PersonList.AsQueryable<Models.Customer>();
            }
        }

    }
}
