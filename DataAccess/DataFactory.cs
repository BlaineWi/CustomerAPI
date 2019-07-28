using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace customerAPI.DataAccess
{
    /// <summary>
    /// Data Factory
    /// </summary>
    public static class DataFactory
    {
        private const int PeopleCount = 100;

        private static List<Models.Person> _list = null;

        /// <summary>
        /// Person List
        /// </summary>
        public static List<Models.Person> PersonList
        {
            get
            {
                if(_list == null)
                {
                    _list = new List<Models.Person>();
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
        public static IQueryable<Models.Person> People
        {
            get
            {
                return  PersonList.AsQueryable<Models.Person>();
            }
        }

    }
}
