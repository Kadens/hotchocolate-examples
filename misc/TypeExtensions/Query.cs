namespace ExtendingTypes;

public class Query
{
    public Book GetBook()
        => new Book(1, "C# in depth.", new Author(2, "Jon Skeet"));
}

public record Book(int Id, string Title, Author Author) : IMarker;

public record Author(int Id, string Name) : IMarker;

public interface IMarker { }

[ExtendObjectType(typeof(Book))]
public sealed class BookExtensions1
{
    public IEnumerable<string> GetGenres([Parent] Book book)
    {
        List<string> Generes=new List<string>();
        Generes.Add("Tech");
        Generes.Add("Fag");
        return Generes.AsEnumerable<string>();
        // Omitted code for brevity
    }
    public string GetCategory([Parent] Book book)
        => "Technical";
}

[ExtendObjectType(typeof(Author))]
public sealed class AuthorExtensions1
{
    public Uri GetProfilePicture([Parent] Author author)
        => new Uri($"http://localhost/{author.Name}");
}

[ExtendObjectType("Author")]
public sealed class AuthorExtensions2
{
    public string Notes => "Has some notes about the author.";
}

[Node]
[ExtendObjectType(typeof(IMarker))]
public sealed class GeneralExtensions : IFoo
{
    public string Foo => "hello";

    public async Task<string?> GetBio([Parent] Author author)
    {
        string fileName = $"./bio_{author.Id}.txt";

        if (File.Exists(fileName))
        {
            return await File.ReadAllTextAsync(fileName);
        }

        return null;
    }

    public bool IsAuthor([Parent] IMarker marker) => marker is Author;

    [NodeResolver]
    public static IMarker GetNode() => null;
}

[InterfaceType]
public interface IFoo
{
    string Foo { get; }
}

public class AuthorCodeFirstExtension1 : ObjectTypeExtension<Author>
{
    protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
    {
        descriptor.Field("bar").Resolve("baz");
    }
}

public class AuthorCodeFirstExtension2 : ObjectTypeExtension
{
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor.Name("Author");
        descriptor.Field("baz").Resolve("qux");
    }
}