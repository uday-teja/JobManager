using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiModel = TaskManagement.Model.Model;
using DataModel = TaskManagement.Data.Model;

namespace TaskManagement
{
    public class BootstrapMapper
    {
        public static void InitializeAutoMapper()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<UiModel.Task, DataModel.Task>();
                x.CreateMap<DataModel.Task, UiModel.Task>();
                x.CreateMap<UiModel.Task, UiModel.Task>();
                x.CreateMap<DataModel.Task, DataModel.Task>();
            });
        }
    }
}