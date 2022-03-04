dotnet ef migrations add InitialCreate -s ApollosLibrary.WebApi -p ApollosLibrary.Domain --context ApollosLibraryContext

dotnet ef database update -s ApollosLibrary.WebApi -p ApollosLibrary.Domain --context ApollosLibraryContext