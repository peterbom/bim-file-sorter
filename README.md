# BIM file JSON sorter

Ensures a consistent ordering of a number of JSON arrays within a PowerBI `BIM` file.

This is to enable easier comparisons between versions of these files, and reduce the likelihood of merge conflicts.

The specific array properties which are re-ordered are:

```
{
  "model": {
    "tables": [
      {
        "name": "<tables are ordered by name>",
        "columns": [
          {
            "name": "<columns are ordered by name>",
            ...
          }
        ],
        "measures": [
          {
            "name": "<measures are ordered by name>",
            ...
          }
        ],
        ...
      },
      ...
    ],
    "relationships": [
      {
        "name": "<relationships are ordered by name>",
        ...
      },
      ...
    ],
    ...
  },
  ...
}
```

## Building

Uses [Costura](https://github.com/Fody/Costura) to package all dependencies into a single executable, `formatjson.exe`, for easier distribution.
