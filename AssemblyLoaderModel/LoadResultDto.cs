﻿using System;
using System.Collections.Generic;

namespace Mmvm.Assembly.Loader.Model
{
    public class LoadResultDto
    {
        #region Constructors

        public LoadResultDto(ICollection<Type> loadedTypes, ICollection<string> assembliesPaths)
        {
            LoadedTypes = loadedTypes;
            AssembliesPaths = assembliesPaths;
        }

        #endregion

        #region Public properties

        public ICollection<Type> LoadedTypes { get; }

        public ICollection<string> AssembliesPaths { get; }

        #endregion
    }
}