dotnet ef migrations add AddedCreatedFieldsToReports -s ApollosLibrary.WebApi -p ApollosLibrary.Domain --configuration Development --context ApollosLibraryContext

dotnet ef database update -s ApollosLibrary.WebApi -p ApollosLibrary.Domain --context ApollosLibraryContext