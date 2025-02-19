﻿namespace ProductsApi.DTOs;

public class PaginatedResponse<T>
{
    public List<T> Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}

