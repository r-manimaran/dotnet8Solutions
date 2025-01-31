SELECT OBJECT_NAME(object_id, db_id('UUID_Benchmark')) as object_name,
    ( CASE 
		WHEN OBJECT_NAME(object_id,db_id('UUID_Benchmark')) ='Table1' THEN 'Table1_Int'
		WHEN OBJECT_NAME(object_id,db_id('UUID_Benchmark')) ='Table2' THEN 'Table2_Guid'
		WHEN OBJECT_NAME(object_id,db_id('UUID_Benchmark')) ='Table3' THEN 'Table3_Ulid'
		WHEN OBJECT_NAME(object_id,db_id('UUID_Benchmark')) ='Table4' THEN 'Table4_UlidBinary'
		ELSE 'Table5_DateTime'
		END) as table_name,
		* 
		FROM sys.dm_db_index_physical_stats(db_id('UUID_Benchmark'), NULL,NULL,NULL,NULL)
		WHERE OBJECT_NAME(object_id, db_id('UUID_Benchmark')) in ('Table1','Table2','Table3','Table4','Table5');