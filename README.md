[![Build status](https://ci.appveyor.com/api/projects/status/ccooswpu1m658409?svg=true)](https://ci.appveyor.com/project/jusbuc2k/csg-data-listquery)

# Introduction 
Provides a structured way to build components that return lists of data.

# Projects

# List Query API Standard

A list query request is submitted via an HTTP GET via query string and HTTP POST via a JSON payload.

## Request

### HTTP GET Syntax

An List Query API complient endpoint **MUST** accept an HTTP GET request with
query string parameters defined as follows.

#### Field Selection
Field selection is made using the ```fields``` parameter: 

```?fields=field_list```

field_list is a comma separated list of the fields names to be returned. For example:

```?fields=FirstName,LastName,BirthDate,Grade```

The ```fields``` parameter may only be specified once.

#### Filtering
Filters are applied via the ```where``` parameter as follows:

```?where[filter_name]=operator_prefix:value```

Where field_name is a supported filter, operator_prefix is a supported operation from the table below, and value is the matching expression.

| Operation                     | Prefix         | Example                                  |
|-------------------------------|---------------|------------------------------------------|
| Equals                        | `eq`          | `where[FirstName]=eq:Bob`           |
| Not Equals                    | `ne`          | `where[FirstName]=ne:Sally`         |
| Less Than                     | `lt`          | `where[Price]=lt:10`                |
| Greater Than                  | `gt`          | `where[Price]=gt:10`                |
| Less Than Or Equal To         | `le`          | `where[Price]=le:10`                |
| Greater Than Or Equal To      | `ge`          | `where[Price]=ge:10`                |
| Like (string comparison)      | `like`        | `where[LastName]=like:sanders`     |
| Is Null                       | `isnull`      | `where[attribute]=isnull:true`     |
| Is Not Null                   | `isnull`      | `where[attribute]=isnull:false`    |

If the operator prefix is omitted 'eq' (Equals) will be used. The following operators are supported:

Any number of ```where[filter_name]``` parameters maybe applied. When multiple filters are applied, the returned
records will match **ALL** all applied filters.

Wildcards may abe applied to the value specified for the ```like``` operator using '*' to match any number
of characters, and '?' to match any single character. For example:

```where[FirstName]=like:sally*``` matches any record where FirstName starts with sally.

#### Sorting
Sorting may be applied using the ```order``` parmeter:

```?order=expression```

Where expression is a comma separated list of fields indicating the order in which sorting should
be performed. The default sort order is Ascending. Descending sort order can be indicated by
prefixing the field name with a dash (-). 

Examples:

```?order=LastName,FirstName``` sorts by LastNane then FirstName
```?order=-BirthDate,LastName``` sorts by BirthDate descending and LastName ascending

The ```order``` parameter may only be specified once.

#### Paging
Paging may be applied by a combination of the ```limit``` and ```offset``` parameters. 

```?limit=page_size&offset=record_index```

page_size is the number of records to return in a single request.
record_index is the zero-based index of the first record of the desired page. Typically this is specified in multiples of page_size.

When the ```offset``` parameter is specified, ```limit``` must always be specified.

Both ```offset``` and ```limit``` may only be specified once.

Examples:
```?offset=0&limit=10``` returns the first page of data in page sizes of 10 records each.
```?offset=50&limit=10``` returns the 6th page of data in page sizes of 10 records each.

#### Combined Examples
Parameters are combined in standard query string format, with ampersands between.

Example:
Find all records where Birth Date is greater than or equal to 2000-01-01, FirstName starts with
Sally, selecting only BirthDate, FirstName, and LastName, ordering by BirthDate descending, then LastName,
and taking the first page of results, with a page size of 50.

```?fields=FirstName,LastName,BirthDate&where[BirthDate]=ge:2000-01-01&where[FirstName]=like:Sally*&order=-BirthDate,LastName&offset=0&limit=50```

## HTTP Post
A ListQuery API compliant endpoint **may** support the use of an HTTP POST with a JSON document as the body as follows:


```json
{
    "fields": [
        "Field1Name",
        "Field2Name"
    ],
    "filters": [
        { 
            "Name": "FilterName", 
            "Operator": "Equal|NotEqual|GreaterThan|GreaterThanOrEqual|LessThan|LessThanOrEqual|Between|Like|IsNull", 
            "Value": "value or array of values" 
        }
    ],
    "order": [
        { 
            "Name": "Field2Name", 
            "SortDescending": "true or false"
        }
    ],
    "offset": "Int32",
    "limit": "Int32"
}  
```

The HTTP POST method typically supports several features that the HTTP GET request does not:

### Multiple filter values in a single filter

```{ "Name": "ProductID", "Operator": "Equal", "Value": [1,2,3,4,5,6,7,8,9,10] }```

The above filter would match any record where the ProductID is any of the values 1,2,3,4,5,6,7,8,9 or 10.

This only works with the operators Equal, NotEqual, GreaterThan, GreaterThanOrEqual, LessThan, LessThanOrEqual and Like.

### The Between operator
You can, of course, get the same result with the HTTP GET syntax using two filters.

```{ "Name": "Price", "Operator": "Beween", "Value": [50,100] }```

The above filter woudl match any record where Price is between 50 and 100 and is equivalent 
to the HTTP GET syntax of ```?where[Price]=ge:50&where[Price]=le:100

# Response Format

The List Query API Standard response is derived from, but is not *exactly*
identical to the [JSON API specification](https://jsonapi.org).

A List Query API standard respose will have a JSON HTTP body as follows:

```json
{
    "data": [
        { "FirstName": "Bob", "LastName": "Smith", "BirthDate": "2000-01-01" },
        { "FirstName": "Sally", "LastName": "Sanders", "BirthDate": "2001-05-23" },
    ],
    "links": {
        "next": "https://example.com/api/Widget/filter?offset=10&limit=10",
        "self": "https://example.com/api/Widget/filter?offset=0&limit=10",
        "prev": null
    },
    "meta": {
        "next": 10,
        "prev": null,
        "currentCount": 10,
        "totalCount": 125,
        "fields": [
            "FirstName",
            "LastName",
            "BirthDate"
        ]
    },
}
```