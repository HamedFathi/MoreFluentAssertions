![fa](https://user-images.githubusercontent.com/8418700/147836939-bddb4cea-c9c6-41c0-8c32-7aca81e0e24e.png)

### [Nuget](https://www.nuget.org/packages/MoreFluentAssertions/)

[![Open Source Love](https://badges.frapsoft.com/os/mit/mit.svg?v=102)](https://opensource.org/licenses/MIT)
![Nuget](https://img.shields.io/nuget/v/MoreFluentAssertions)
![Nuget](https://img.shields.io/nuget/dt/MoreFluentAssertions)

```
Install-Package MoreFluentAssertions

dotnet add package MoreFluentAssertions
```

### HaveSameSchemaAs

Compare schema of two json text, return `true` if they are same.

When two schemas are same:
1. Json Paths are same.
2. Types are same.
 
In this comparison we never check values.

```cs
[Fact]
public void CHECK_JSON_SCHEMA()
{
    var actual = @"{
                    ""a"": [{
                            ""b"": ""bb"",
                            ""c"": ""cc"",
                            ""d"": ""dd""
                        }
                    ],
                    ""e"": ""eeeeeee"",
                    ""f"": {
                        ""g"": [{
                                ""h"": ""hh"",
                                ""i"": ""ii"",
                                ""j"": null,
                                ""k"": true,
                                ""l"": false,
                                ""m"": ""2021-12-29T14:20:21.948Z"",
                                ""n"": 10
                            }
                        ]
                    }
                }";

    var expected = @"{
                    ""a"": [{
                            ""b"": ""x"",
                            ""c"": ""y"",
                            ""d"": ""z""
                        }
                    ],
                    ""e"": ""e7"",
                    ""f"": {
                        ""g"": [{
                                ""h"": ""hh"",
                                ""i"": ""ii"",
                                ""j"": ""abc"",
                                ""k"": false,
                                ""l"": true,
                                ""m"": ""2000-02-19T10:33:33.371Z"",
                                ""n"": 10
                            }
                        ]
                    }
                }";


    var actualDoc = JsonDocument.Parse(actual);
    var expectedDoc = JsonDocument.Parse(expected);

    actualDoc.Should().HaveSameSchemaAs(expectedDoc);
}
```

`HaveSameSchemaAs` has some rules for checking types:
1. `true` and `false` are considered `Boolean` type.
2. A string value parses for detecting `Date` type, if it was true, it considered as `Date` type.
3. For pathes with `null` or `undefined`, it just compares `paths` becuase it does not know the real type for the values.
