﻿namespace ESCenter.Application.Contract.Users.BasicUsers;
public class UserForCreateDto : UserForDetailDto
{
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
    public List<string> Majors { get; set; } = new();
}

