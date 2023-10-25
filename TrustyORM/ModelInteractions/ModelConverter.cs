﻿using System.Data.Common;
using System.Reflection;
using System.Runtime.CompilerServices;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions;

internal class ModelConverter<T>
{
    private readonly DbDataReader _dataReader;
    private readonly bool _isSystemType;
    private readonly TableProperties<T> _tableProperties;

    internal ModelConverter(DbDataReader dataReader)
    {
        _dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
        _isSystemType = typeof(T).IsSystemType();
        _tableProperties = new TableProperties<T>();
    }

    public Type ElementType => typeof(T);

    public bool IsSystemType => _isSystemType;

    /// <summary>
    /// Получение объекта из таблицы
    /// </summary>
    /// <param name="dataReader"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetObject(DbDataReader dataReader)
    {
        if (_isSystemType)
        {
            return dataReader.GetFieldValue<T>(0);
        }

        T newObject = Activator.CreateInstance<T>();

        foreach (KeyValuePair<PropertyInfo, ColumnAttribute> currentProperty in _tableProperties)
        {
            currentProperty.SetDataReaderValue(newObject, dataReader);
        }

        return newObject;
    }
}