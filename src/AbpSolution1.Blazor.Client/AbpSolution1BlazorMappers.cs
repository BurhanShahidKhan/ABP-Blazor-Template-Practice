using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using AbpSolution1.Books;
using AbpSolution1.TodoItems;

namespace AbpSolution1.Blazor.Client;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AbpSolution1BlazorMappers /*: MapperBase<BookDto, CreateUpdateBookDto>*/
{
    public /*override*/ partial CreateUpdateBookDto Map(BookDto source);

    public /*override*/ partial void Map(BookDto source, CreateUpdateBookDto destination);


    public partial CreateUpdateTodoItemDto Map(TodoItemDto source);

    public partial void Map(TodoItemDto source, CreateUpdateTodoItemDto destination);
}

//public partial class AbpSolution1BlazorMappers : MapperBase<TodoItemDto, CreateUpdateTodoItemDto>
//{
//    public override partial CreateUpdateTodoItemDto Map(TodoItemDto source);

//    public override partial void Map(TodoItemDto source, CreateUpdateTodoItemDto destination);
//}
