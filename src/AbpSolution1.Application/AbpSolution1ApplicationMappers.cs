using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using AbpSolution1.Books;
using AbpSolution1.TodoItems;

namespace AbpSolution1;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AbpSolution1BookToBookDtoMapper : MapperBase<Book, BookDto>
{
    public override partial BookDto Map(Book source);

    public override partial void Map(Book source, BookDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AbpSolution1CreateUpdateBookDtoToBookMapper : MapperBase<CreateUpdateBookDto, Book>
{
    public override partial Book Map(CreateUpdateBookDto source);

    public override partial void Map(CreateUpdateBookDto source, Book destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AbpSolution1TodoItemToTodoItemDtoMapper : MapperBase<TodoItem, TodoItemDto>
{
    public override partial TodoItemDto Map(TodoItem source);

    public override partial void Map(TodoItem source, TodoItemDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AbpSolution1CreateUpdateTodoItemDtoToTodoItemMapper : MapperBase<CreateUpdateTodoItemDto, TodoItem>
{
    public override partial TodoItem Map(CreateUpdateTodoItemDto source);

    public override partial void Map(CreateUpdateTodoItemDto source, TodoItem destination);
}
