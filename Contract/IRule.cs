﻿namespace Contract
{
    public interface IRule
    {
        string Rename(string originName);
        IRule? Parse(string data);
        string Name { get; }
        string Description { get; }
        bool IsChecked { get; set; }
    }
}