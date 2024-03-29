﻿using BTogether.BussinessLayer.BaseServices;
using BTogether.Models;

namespace BTogether.BussinessLayer.IServices
{
    public interface IImageMemoryService : IBaseServices<ImageMemory>
    {
        Task<IEnumerable<ImageMemory>> GetImageMemoriesByStoryIdAsync(int storyId);
    }

}
