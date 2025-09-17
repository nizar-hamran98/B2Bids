using System.Runtime.Serialization;
using System.Xml.Serialization;
namespace SharedKernel
{
    public class SearchOptionsInput
    {
        int pageSize = 10;
        public SearchOptions SearchOptions { get; set; }
        public int PageSize
        {

            get
            {
                if (pageSize < 0)
                {
                    pageSize = 10;
                }
                return pageSize;
            }
            set
            {
                pageSize = value;
            }
        }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public List<string>? Includes { get; set; }
        public SearchOptionsInput()
        {
            SearchOptions = new SearchOptions();
        }


    }
    public class SearchQueryInput
    {
        int pageSize = 10;
        public SearchQuery SearchQuery { get; set; }
        public int PageSize
        {

            get
            {
                if (pageSize <= 0)
                {
                    pageSize = 10;
                }
                return pageSize;
            }
            set
            {
                if (value > 0)
                {
                    pageSize = value;
                }
            }
        }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public SearchQueryInput()
        {
            SearchQuery = new SearchQuery();
        }
    }
    public partial class SearchOptions
    {
        public SearchOptions()
        {
            FilterOptions = new List<FilterByOptions>();
            OrderOptions = new List<OrderByOptions>();
        }
        public List<FilterByOptions> FilterOptions { get; set; }

        public List<OrderByOptions> OrderOptions { get; set; }

        public eSearchMatchType SearchMatchType { get; set; }
        public int PageSize { get; set; } = 0;
        public int PageIndex { get; set; } = 0;
        public short? Period { get; set; }
    }
    [KnownType(typeof(List<string>))]
    [KnownType(typeof(List<int>))]
    [KnownType(typeof(List<decimal>))]
    [KnownType(typeof(List<Guid>))]
    [KnownType(typeof(List<Guid?>))]
    [KnownType(typeof(int))]
    [KnownType(typeof(string))]
    [KnownType(typeof(bool))]
    public partial class FilterByOptions
    {
        public FilterByOptions(string memberName, eFilterOperator filterOperator, object? filterFor, params System.Enum[] properties)
        {
            MemberName = memberName;
            FilterFor = filterFor;
            FilterOperator = filterOperator;
            Properties = properties?.ToList() ?? new List<System.Enum>();
        }
        public FilterByOptions()
        {
        }
        [XmlElement("xmlInteger", Type = typeof(int))]
        [XmlElement("xmlString", Type = typeof(string))]
        [XmlElement("xmlGuid", Type = typeof(Guid))]
        [XmlElement("xmlBoolean", Type = typeof(bool))]
        [XmlElement("xmlIntegerList", Type = typeof(List<int>))]
        [XmlElement("xmlDecimalList", Type = typeof(List<decimal>))]
        [XmlElement("xmlStringList", Type = typeof(List<string>))]
        [XmlElement("xmlGuidList", Type = typeof(List<Guid>))]
        [XmlElement("xmlEmptyGuidList", Type = typeof(List<Guid?>))]
        public object? FilterFor { get; set; }
        public eFilterOperator FilterOperator { get; set; }
        public string MemberName { get; set; }

        private List<System.Enum> properties;
        public List<System.Enum> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new List<System.Enum>();
                }
                return properties;
            }
            set
            {
                if (value == null)
                {
                    properties = new List<System.Enum>();
                }
                else
                {
                    properties = value;
                }
            }
        }
    }
    public enum eFilterOperator : int
    {
        EqualsTo = 1,
        NotEqualsTo = 2,
        BeginsWith = 3,
        Contains = 4,
        GreaterThan = 5,
        GreaterThanOrEquals = 6,
        LessThan = 7,
        LessThanOrEquals = 8,
        EqualsToList = 9,
        NotEqualsToList = 10,
        NotBeginsWith = 11,
        NotContains = 12
    }
    public partial class OrderByOptions : object
    {
        public string MemberName { get; set; }
        public Order SortOrder { get; set; }
        public enum Order : int
        {
            ASC = 1,
            DEC = 2,
        }
    }
    public enum eSearchMatchType : int
    {
        MatchAll = 0,
        MatchAny = 1,
    }
    public class SearchQuery
    {
        public ICollection<SearchCondition> Conditions { get; set; } = new List<SearchCondition>();
        public SearchQuery()
        {
        }
        public SearchQuery(SearchCondition searchCondition)
        {
            Conditions.Add(searchCondition);
        }
        public SearchQuery(FilterByOptions filterByOptions)
        {
            Conditions.Add(new SearchCondition(filterByOptions));
        }
        public SearchQuery Exclude(string[] startingWith)
        {
            var _sq = new SearchQuery();
            foreach (var _c in Conditions)
                _sq.Conditions.Add(_c.Exclude(startingWith));
            return _sq;
        }
        public SearchQuery Keep(string[] startingWith, bool removeStartingWith = true)
        {
            var _sq = new SearchQuery();
            foreach (var _c in Conditions)
                _sq.Conditions.Add(_c.Keep(startingWith, removeStartingWith));
            return _sq;
        }
        public SearchQuery Merge(SearchQuery sourceSearchQuery)
        {
            var _result = new SearchQuery();

            foreach (var _cd in Conditions)
            {
                if (sourceSearchQuery.Conditions.Count > 0)
                {
                    foreach (var _cs in sourceSearchQuery.Conditions)
                    {
                        var _newcd = new SearchCondition(_cd.Criteria);
                        _result.Conditions.Add(_newcd);
                        foreach (var _fo in _cs.Criteria)
                            _newcd.Criteria.Add(_fo);
                    }
                }
                else
                {
                    var _newcd = new SearchCondition(_cd.Criteria);
                    _result.Conditions.Add(_newcd);
                }
            }
            return _result;
        }
    }
    public class SearchCondition
    {
        public ICollection<FilterByOptions> Criteria { get; set; } = new List<FilterByOptions>();
        public SearchCondition()
        {
        }
        public SearchCondition(FilterByOptions filterByOptions)
        {
            Criteria.Add(filterByOptions);
        }
        public SearchCondition(IEnumerable<FilterByOptions> filterByOptions)
        {
            foreach (var _i in filterByOptions)
                Criteria.Add(_i);
        }
        public SearchCondition Exclude(string[] startingWith)
        {
            var _sc = new SearchCondition();
            foreach (var _fo in Criteria)
            {
                foreach (var _s in startingWith)
                {
                    if (_fo.MemberName.StartsWith(_s, StringComparison.InvariantCultureIgnoreCase))
                        break;
                    _sc.Criteria.Add(_fo);
                }
            }
            return _sc;
        }
        public SearchCondition Keep(string[] startingWith, bool removeStartingWith = true)
        {
            var _sc = new SearchCondition();
            foreach (var _fo in Criteria)
            {
                foreach (var _s in startingWith)
                {
                    if (_fo.MemberName.StartsWith(_s, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var _nfo = new FilterByOptions { MemberName = _fo.MemberName.Remove(0, _s.Length), FilterOperator = _fo.FilterOperator, FilterFor = _fo.FilterFor };
                        _sc.Criteria.Add(_nfo);
                        break;
                    }
                }
            }
            return _sc;
        }
    }
    public partial class FilterByOperator
    {
        public FilterByOperator(eFilterOperator filterOperator, object filterFor)
        {
            FilterFor = filterFor;
            FilterOperator = filterOperator;
        }
        public FilterByOperator()
        {

        }
        [XmlElement("xmlInteger", Type = typeof(int))]
        [XmlElement("xmlString", Type = typeof(string))]
        [XmlElement("xmlGuid", Type = typeof(Guid))]
        [XmlElement("xmlBoolean", Type = typeof(bool))]
        [XmlElement("xmlIntegerList", Type = typeof(List<int>))]
        [XmlElement("xmlDecimalList", Type = typeof(List<decimal>))]
        [XmlElement("xmlStringList", Type = typeof(List<string>))]
        [XmlElement("xmlGuidList", Type = typeof(List<Guid>))]
        [XmlElement("xmlEmptyGuidList", Type = typeof(List<Guid?>))]
        public object FilterFor { get; set; }
        public eFilterOperator FilterOperator { get; set; }
    }
    public partial class FilterByOperator<T>
    {
        public FilterByOperator(eFilterOperator filterOperator, T filterFor)
        {
            FilterFor = filterFor;
            FilterOperator = filterOperator;
        }
        public FilterByOperator()
        {

        }
        [XmlElement("xmlInteger", Type = typeof(int))]
        [XmlElement("xmlString", Type = typeof(string))]
        [XmlElement("xmlGuid", Type = typeof(Guid))]
        [XmlElement("xmlBoolean", Type = typeof(bool))]
        [XmlElement("xmlIntegerList", Type = typeof(List<int>))]
        [XmlElement("xmlDecimalList", Type = typeof(List<decimal>))]
        [XmlElement("xmlStringList", Type = typeof(List<string>))]
        [XmlElement("xmlGuidList", Type = typeof(List<Guid>))]
        [XmlElement("xmlEmptyGuidList", Type = typeof(List<Guid?>))]
        public T FilterFor { get; set; }
        public eFilterOperator FilterOperator { get; set; }
    }
}
