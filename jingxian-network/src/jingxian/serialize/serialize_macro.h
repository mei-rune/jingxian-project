#define register_class_1( object, member0 ) \
inline bool serialize_##object(  serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize(writer, context, s1.##member0, #member0) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize(reader, context, s1.##member0, #member0) \
  && reader.close(context); } \
inline bool deserialize(  serialize_reader & reader,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_with_type_1( object, member0, member_type0 ) \
inline bool serialize_##object(   serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize_##member_type0(writer, context, s1.##member0, #member0) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize_##member_type0(reader, context, s1.##member0, #member0) \
  && writer.close(context); } \
inline bool deserialize(  serialize_reader & reader,  serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_2( object, member0, member1 ) \
inline bool serialize_##object(  serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize(writer, context, s1.##member0, #member0) \
  && serialize(writer, context, s1.##member1, #member1) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize(reader, context, s1.##member0, #member0) \
  && deserialize(reader, context, s1.##member1, #member1) \
  && reader.close(context); } \
inline bool deserialize(  serialize_reader & reader,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_with_type_2( object, member0, member_type0, member1, member_type1 ) \
inline bool serialize_##object(   serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize_##member_type0(writer, context, s1.##member0, #member0) \
  && serialize_##member_type1(writer, context, s1.##member1, #member1) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize_##member_type0(reader, context, s1.##member0, #member0) \
  && deserialize_##member_type1(reader, context, s1.##member1, #member1) \
  && writer.close(context); } \
inline bool deserialize(  serialize_reader & reader,  serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_3( object, member0, member1, member2 ) \
inline bool serialize_##object(  serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize(writer, context, s1.##member0, #member0) \
  && serialize(writer, context, s1.##member1, #member1) \
  && serialize(writer, context, s1.##member2, #member2) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize(reader, context, s1.##member0, #member0) \
  && deserialize(reader, context, s1.##member1, #member1) \
  && deserialize(reader, context, s1.##member2, #member2) \
  && reader.close(context); } \
inline bool deserialize(  serialize_reader & reader,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_with_type_3( object, member0, member_type0, member1, member_type1, member2, member_type2 ) \
inline bool serialize_##object(   serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize_##member_type0(writer, context, s1.##member0, #member0) \
  && serialize_##member_type1(writer, context, s1.##member1, #member1) \
  && serialize_##member_type2(writer, context, s1.##member2, #member2) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize_##member_type0(reader, context, s1.##member0, #member0) \
  && deserialize_##member_type1(reader, context, s1.##member1, #member1) \
  && deserialize_##member_type2(reader, context, s1.##member2, #member2) \
  && writer.close(context); } \
inline bool deserialize(  serialize_reader & reader,  serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_4( object, member0, member1, member2, member3 ) \
inline bool serialize_##object(  serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize(writer, context, s1.##member0, #member0) \
  && serialize(writer, context, s1.##member1, #member1) \
  && serialize(writer, context, s1.##member2, #member2) \
  && serialize(writer, context, s1.##member3, #member3) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize(reader, context, s1.##member0, #member0) \
  && deserialize(reader, context, s1.##member1, #member1) \
  && deserialize(reader, context, s1.##member2, #member2) \
  && deserialize(reader, context, s1.##member3, #member3) \
  && reader.close(context); } \
inline bool deserialize(  serialize_reader & reader,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_with_type_4( object, member0, member_type0, member1, member_type1, member2, member_type2, member3, member_type3 ) \
inline bool serialize_##object(   serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize_##member_type0(writer, context, s1.##member0, #member0) \
  && serialize_##member_type1(writer, context, s1.##member1, #member1) \
  && serialize_##member_type2(writer, context, s1.##member2, #member2) \
  && serialize_##member_type3(writer, context, s1.##member3, #member3) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize_##member_type0(reader, context, s1.##member0, #member0) \
  && deserialize_##member_type1(reader, context, s1.##member1, #member1) \
  && deserialize_##member_type2(reader, context, s1.##member2, #member2) \
  && deserialize_##member_type3(reader, context, s1.##member3, #member3) \
  && writer.close(context); } \
inline bool deserialize(  serialize_reader & reader,  serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_5( object, member0, member1, member2, member3, member4 ) \
inline bool serialize_##object(  serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize(writer, context, s1.##member0, #member0) \
  && serialize(writer, context, s1.##member1, #member1) \
  && serialize(writer, context, s1.##member2, #member2) \
  && serialize(writer, context, s1.##member3, #member3) \
  && serialize(writer, context, s1.##member4, #member4) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize(reader, context, s1.##member0, #member0) \
  && deserialize(reader, context, s1.##member1, #member1) \
  && deserialize(reader, context, s1.##member2, #member2) \
  && deserialize(reader, context, s1.##member3, #member3) \
  && deserialize(reader, context, s1.##member4, #member4) \
  && reader.close(context); } \
inline bool deserialize(  serialize_reader & reader,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_with_type_5( object, member0, member_type0, member1, member_type1, member2, member_type2, member3, member_type3, member4, member_type4 ) \
inline bool serialize_##object(   serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize_##member_type0(writer, context, s1.##member0, #member0) \
  && serialize_##member_type1(writer, context, s1.##member1, #member1) \
  && serialize_##member_type2(writer, context, s1.##member2, #member2) \
  && serialize_##member_type3(writer, context, s1.##member3, #member3) \
  && serialize_##member_type4(writer, context, s1.##member4, #member4) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize_##member_type0(reader, context, s1.##member0, #member0) \
  && deserialize_##member_type1(reader, context, s1.##member1, #member1) \
  && deserialize_##member_type2(reader, context, s1.##member2, #member2) \
  && deserialize_##member_type3(reader, context, s1.##member3, #member3) \
  && deserialize_##member_type4(reader, context, s1.##member4, #member4) \
  && writer.close(context); } \
inline bool deserialize(  serialize_reader & reader,  serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_6( object, member0, member1, member2, member3, member4, member5 ) \
inline bool serialize_##object(  serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize(writer, context, s1.##member0, #member0) \
  && serialize(writer, context, s1.##member1, #member1) \
  && serialize(writer, context, s1.##member2, #member2) \
  && serialize(writer, context, s1.##member3, #member3) \
  && serialize(writer, context, s1.##member4, #member4) \
  && serialize(writer, context, s1.##member5, #member5) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize(reader, context, s1.##member0, #member0) \
  && deserialize(reader, context, s1.##member1, #member1) \
  && deserialize(reader, context, s1.##member2, #member2) \
  && deserialize(reader, context, s1.##member3, #member3) \
  && deserialize(reader, context, s1.##member4, #member4) \
  && deserialize(reader, context, s1.##member5, #member5) \
  && reader.close(context); } \
inline bool deserialize(  serialize_reader & reader,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_with_type_6( object, member0, member_type0, member1, member_type1, member2, member_type2, member3, member_type3, member4, member_type4, member5, member_type5 ) \
inline bool serialize_##object(   serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize_##member_type0(writer, context, s1.##member0, #member0) \
  && serialize_##member_type1(writer, context, s1.##member1, #member1) \
  && serialize_##member_type2(writer, context, s1.##member2, #member2) \
  && serialize_##member_type3(writer, context, s1.##member3, #member3) \
  && serialize_##member_type4(writer, context, s1.##member4, #member4) \
  && serialize_##member_type5(writer, context, s1.##member5, #member5) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize_##member_type0(reader, context, s1.##member0, #member0) \
  && deserialize_##member_type1(reader, context, s1.##member1, #member1) \
  && deserialize_##member_type2(reader, context, s1.##member2, #member2) \
  && deserialize_##member_type3(reader, context, s1.##member3, #member3) \
  && deserialize_##member_type4(reader, context, s1.##member4, #member4) \
  && deserialize_##member_type5(reader, context, s1.##member5, #member5) \
  && writer.close(context); } \
inline bool deserialize(  serialize_reader & reader,  serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_7( object, member0, member1, member2, member3, member4, member5, member6 ) \
inline bool serialize_##object(  serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize(writer, context, s1.##member0, #member0) \
  && serialize(writer, context, s1.##member1, #member1) \
  && serialize(writer, context, s1.##member2, #member2) \
  && serialize(writer, context, s1.##member3, #member3) \
  && serialize(writer, context, s1.##member4, #member4) \
  && serialize(writer, context, s1.##member5, #member5) \
  && serialize(writer, context, s1.##member6, #member6) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize(reader, context, s1.##member0, #member0) \
  && deserialize(reader, context, s1.##member1, #member1) \
  && deserialize(reader, context, s1.##member2, #member2) \
  && deserialize(reader, context, s1.##member3, #member3) \
  && deserialize(reader, context, s1.##member4, #member4) \
  && deserialize(reader, context, s1.##member5, #member5) \
  && deserialize(reader, context, s1.##member6, #member6) \
  && reader.close(context); } \
inline bool deserialize(  serialize_reader & reader,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_with_type_7( object, member0, member_type0, member1, member_type1, member2, member_type2, member3, member_type3, member4, member_type4, member5, member_type5, member6, member_type6 ) \
inline bool serialize_##object(   serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize_##member_type0(writer, context, s1.##member0, #member0) \
  && serialize_##member_type1(writer, context, s1.##member1, #member1) \
  && serialize_##member_type2(writer, context, s1.##member2, #member2) \
  && serialize_##member_type3(writer, context, s1.##member3, #member3) \
  && serialize_##member_type4(writer, context, s1.##member4, #member4) \
  && serialize_##member_type5(writer, context, s1.##member5, #member5) \
  && serialize_##member_type6(writer, context, s1.##member6, #member6) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize_##member_type0(reader, context, s1.##member0, #member0) \
  && deserialize_##member_type1(reader, context, s1.##member1, #member1) \
  && deserialize_##member_type2(reader, context, s1.##member2, #member2) \
  && deserialize_##member_type3(reader, context, s1.##member3, #member3) \
  && deserialize_##member_type4(reader, context, s1.##member4, #member4) \
  && deserialize_##member_type5(reader, context, s1.##member5, #member5) \
  && deserialize_##member_type6(reader, context, s1.##member6, #member6) \
  && writer.close(context); } \
inline bool deserialize(  serialize_reader & reader,  serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_8( object, member0, member1, member2, member3, member4, member5, member6, member7 ) \
inline bool serialize_##object(  serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize(writer, context, s1.##member0, #member0) \
  && serialize(writer, context, s1.##member1, #member1) \
  && serialize(writer, context, s1.##member2, #member2) \
  && serialize(writer, context, s1.##member3, #member3) \
  && serialize(writer, context, s1.##member4, #member4) \
  && serialize(writer, context, s1.##member5, #member5) \
  && serialize(writer, context, s1.##member6, #member6) \
  && serialize(writer, context, s1.##member7, #member7) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize(reader, context, s1.##member0, #member0) \
  && deserialize(reader, context, s1.##member1, #member1) \
  && deserialize(reader, context, s1.##member2, #member2) \
  && deserialize(reader, context, s1.##member3, #member3) \
  && deserialize(reader, context, s1.##member4, #member4) \
  && deserialize(reader, context, s1.##member5, #member5) \
  && deserialize(reader, context, s1.##member6, #member6) \
  && deserialize(reader, context, s1.##member7, #member7) \
  && reader.close(context); } \
inline bool deserialize(  serialize_reader & reader,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_with_type_8( object, member0, member_type0, member1, member_type1, member2, member_type2, member3, member_type3, member4, member_type4, member5, member_type5, member6, member_type6, member7, member_type7 ) \
inline bool serialize_##object(   serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize_##member_type0(writer, context, s1.##member0, #member0) \
  && serialize_##member_type1(writer, context, s1.##member1, #member1) \
  && serialize_##member_type2(writer, context, s1.##member2, #member2) \
  && serialize_##member_type3(writer, context, s1.##member3, #member3) \
  && serialize_##member_type4(writer, context, s1.##member4, #member4) \
  && serialize_##member_type5(writer, context, s1.##member5, #member5) \
  && serialize_##member_type6(writer, context, s1.##member6, #member6) \
  && serialize_##member_type7(writer, context, s1.##member7, #member7) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize_##member_type0(reader, context, s1.##member0, #member0) \
  && deserialize_##member_type1(reader, context, s1.##member1, #member1) \
  && deserialize_##member_type2(reader, context, s1.##member2, #member2) \
  && deserialize_##member_type3(reader, context, s1.##member3, #member3) \
  && deserialize_##member_type4(reader, context, s1.##member4, #member4) \
  && deserialize_##member_type5(reader, context, s1.##member5, #member5) \
  && deserialize_##member_type6(reader, context, s1.##member6, #member6) \
  && deserialize_##member_type7(reader, context, s1.##member7, #member7) \
  && writer.close(context); } \
inline bool deserialize(  serialize_reader & reader,  serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_9( object, member0, member1, member2, member3, member4, member5, member6, member7, member8 ) \
inline bool serialize_##object(  serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize(writer, context, s1.##member0, #member0) \
  && serialize(writer, context, s1.##member1, #member1) \
  && serialize(writer, context, s1.##member2, #member2) \
  && serialize(writer, context, s1.##member3, #member3) \
  && serialize(writer, context, s1.##member4, #member4) \
  && serialize(writer, context, s1.##member5, #member5) \
  && serialize(writer, context, s1.##member6, #member6) \
  && serialize(writer, context, s1.##member7, #member7) \
  && serialize(writer, context, s1.##member8, #member8) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize(reader, context, s1.##member0, #member0) \
  && deserialize(reader, context, s1.##member1, #member1) \
  && deserialize(reader, context, s1.##member2, #member2) \
  && deserialize(reader, context, s1.##member3, #member3) \
  && deserialize(reader, context, s1.##member4, #member4) \
  && deserialize(reader, context, s1.##member5, #member5) \
  && deserialize(reader, context, s1.##member6, #member6) \
  && deserialize(reader, context, s1.##member7, #member7) \
  && deserialize(reader, context, s1.##member8, #member8) \
  && reader.close(context); } \
inline bool deserialize(  serialize_reader & reader,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

#define register_class_with_type_9( object, member0, member_type0, member1, member_type1, member2, member_type2, member3, member_type3, member4, member_type4, member5, member_type5, member6, member_type6, member7, member_type7, member8, member_type8 ) \
inline bool serialize_##object(   serialize_writer& writer,   serialize_context& context, object & s1, const tchar* name=0) \
{ return writer.open( context, #object, name) \
  && serialize_##member_type0(writer, context, s1.##member0, #member0) \
  && serialize_##member_type1(writer, context, s1.##member1, #member1) \
  && serialize_##member_type2(writer, context, s1.##member2, #member2) \
  && serialize_##member_type3(writer, context, s1.##member3, #member3) \
  && serialize_##member_type4(writer, context, s1.##member4, #member4) \
  && serialize_##member_type5(writer, context, s1.##member5, #member5) \
  && serialize_##member_type6(writer, context, s1.##member6, #member6) \
  && serialize_##member_type7(writer, context, s1.##member7, #member7) \
  && serialize_##member_type8(writer, context, s1.##member8, #member8) \
  && writer.close( context); } \
inline bool serialize(  serialize_writer& writer,   serialize_context& context, const object & s1, const tchar* name=0) \
{ return serialize_##object(writer, context, s1, name);} \
inline bool deserialize_##object(  serialize_reader & reader,   serialize_context& context, object & s1, const tchar* name=0) \
{ return reader.open( context, #object, name) \
  && deserialize_##member_type0(reader, context, s1.##member0, #member0) \
  && deserialize_##member_type1(reader, context, s1.##member1, #member1) \
  && deserialize_##member_type2(reader, context, s1.##member2, #member2) \
  && deserialize_##member_type3(reader, context, s1.##member3, #member3) \
  && deserialize_##member_type4(reader, context, s1.##member4, #member4) \
  && deserialize_##member_type5(reader, context, s1.##member5, #member5) \
  && deserialize_##member_type6(reader, context, s1.##member6, #member6) \
  && deserialize_##member_type7(reader, context, s1.##member7, #member7) \
  && deserialize_##member_type8(reader, context, s1.##member8, #member8) \
  && writer.close(context); } \
inline bool deserialize(  serialize_reader & reader,  serialize_context& context, const object & s1, const tchar* name=0) \
{ return deserialize_##object(reader, context, s1, name);}

