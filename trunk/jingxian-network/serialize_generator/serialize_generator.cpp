// serialize_generator.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <iostream>
#include <fstream>

#define _namespace_   " "
#define _context_     "serialize_context" 
#define _reader_      "serialize_reader "
#define _writer_      "serialize_writer"

void printRecord( int record , std::ostream& target )
{
	target << "#define register_class_" << record << "( object";
	for( int i = 0; i < record; i ++ )
	{
		target << ", member" << i;
	}
	target << " ) \\" << std::endl;
	target << "inline bool serialize_##object(" _namespace_ " " _writer_ "& writer, " _namespace_ " " _context_ "& context, object & s1, const tchar* name=0) \\" << std::endl;
    target << "{ return writer.open( context, #object, name) \\" << std::endl;
	for( int i = 0; i < record; i ++ )
	{
		target << "  && serialize(writer, context, s1.##member" << i << ", #member" << i << ") \\" << std::endl;
	}
	target << "  && writer.close( context); } \\" << std::endl;
	target << "inline bool serialize(" _namespace_ " " _writer_ "& writer, " _namespace_ " " _context_ "& context, const object & s1, const tchar* name=0) \\" << std::endl;
    target << "{ return serialize_##object(writer, context, s1, name);} \\" << std::endl;
	
	target << "inline bool deserialize_##object(" _namespace_ " " _reader_ "& reader, " _namespace_ " " _context_ "& context, object & s1, const tchar* name=0) \\" << std::endl;
    target << "{ return reader.open( context, #object, name) \\" << std::endl;
	for( int i = 0; i < record; i ++ )
	{
		target << "  && deserialize(reader, context, s1.##member" << i << ", #member" << i << ") \\" << std::endl;
	}
	target << "  && reader.close(context); } \\" << std::endl;
	target << "inline bool deserialize(" _namespace_ " " _reader_ "& reader, " _namespace_ " " _context_ "& context, const object & s1, const tchar* name=0) \\" << std::endl;
    target << "{ return deserialize_##object(reader, context, s1, name);}" << std::endl;

	target << "" << std::endl;
}

void printRecordv2( int record , std::ostream& target )
{
	target << "#define register_class_with_type_" << record << "( object";
	for( int i = 0; i < record; i ++ )
	{
		target << ", member" << i << ", member_type" << i;
	}
	target << " ) \\" << std::endl;

	target << "inline bool serialize_##object( " _namespace_ " " _writer_ "& writer, " _namespace_ " " _context_ "& context, object & s1, const tchar* name=0) \\" << std::endl;
    target << "{ return writer.open( context, #object, name) \\" << std::endl;
	for( int i = 0; i < record; i ++ )
	{
		target << "  && serialize_##member_type" << i<< "(writer, context, s1.##member" << i << ", #member" << i << ") \\" << std::endl;
	}
	target << "  && writer.close( context); } \\" << std::endl;
	target << "inline bool serialize(" _namespace_ " " _writer_ "& writer, " _namespace_ " " _context_ "& context, const object & s1, const tchar* name=0) \\" << std::endl;
    target << "{ return serialize_##object(writer, context, s1, name);} \\" << std::endl;
	
	target << "inline bool deserialize_##object(" _namespace_ " " _reader_ "& reader, " _namespace_ " " _context_ "& context, object & s1, const tchar* name=0) \\" << std::endl;
    target << "{ return reader.open( context, #object, name) \\" << std::endl;
	for( int i = 0; i < record; i ++ )
	{
		target << "  && deserialize_##member_type" << i<< "(reader, context, s1.##member" << i << ", #member" << i << ") \\" << std::endl;
	}
	target << "  && writer.close(context); } \\" << std::endl;
	target << "inline bool deserialize(" _namespace_ " " _reader_ "& reader," _namespace_ " " _context_ "& context, const object & s1, const tchar* name=0) \\" << std::endl;
    target << "{ return deserialize_##object(reader, context, s1, name);}" << std::endl;

	target << "" << std::endl;
}

int _tmain(int argc, _TCHAR* argv[])
{
	if( argc < 3 )
	{
		std::cout << "参数错误!使用方法:" << std::endl;
		std::cout << argv[0] << " 最大字段个数 文件名" << std::endl;
		return -1;
	}

	int maxRecord = atoi( argv[1] );
	std::ofstream st( argv[2] );

	for( int i = 1; i < maxRecord; i ++ )
	{
		printRecord( i ,st );
		printRecordv2( i , st );
	}

	return 0;
}



