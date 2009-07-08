
#ifndef _serialize_context_h_
#define _serialize_context_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
#include "jomoo/exception.hpp"

_jingxian_begin

class serialize_context
{
public:

	enum Type
	{
		Pr
	};

	class string_guard
	{
	public:
		string_guard( serialize_context& context )
			: _context( 0 )
			, _isSequence( false )
		{
			_isSequence = context.isSequenue();
			context.isString( true );
			_context = &context;
		}

		~string_guard()
		{
			if( 0 != _context )
			{
				_context -> isString( false );
				_context -> isSequenue( _isSequence );
				_context = 0;
			}
		}

	private:
		serialize_context* _context;
		bool _isSequence;
	};

	class dictinary_guard
	{
	public:
		dictinary_guard( serialize_context& context )
			: _context( 0 )
			, _isSequence( false )
		{
			_isSequence = context.isSequenue();
			context.isDictinary( true );
			_context = &context;
		}

		~dictinary_guard()
		{
			if( 0 != _context )
			{
				_context -> isDictinary( false );
				_context -> isSequenue( _isSequence );
				_context = 0;
			}
		}

	private:
		serialize_context* _context;
		bool _isSequence;
	};

	class sequenue_guard
	{
	public:
		sequenue_guard( serialize_context& context )
			: _context( 0 )
		{
			_context = &context;
		}

		~sequenue_guard()
		{
			if( 0 != _context )
			{
				_context -> isSequenue( false );
				_context = 0;
			}
		}

	private:
		serialize_context* _context;
	};

	virtual ~serialize_context(){};

	/**
	 * ���л�����һ��
	 */
	virtual void push( const tstring& className, const tstring& fieldName) = 0;

	/**
	 * ���л�����һ��
	 */
	virtual void pop( ) = 0;

	/**
	 * ��ǰ��������
	 */
	virtual const tstring& currentClass() const = 0;

	/**
	 * ��ǰ�����ֶ���
	 */
	virtual const tstring& currentField() const = 0;

	/**
	 * ��ǰ���
	 */
	virtual size_t depth() const = 0;

	/**
	 * ȡ��level��������
	 */
	virtual const tstring& getClass( int level ) const = 0;

	/**
	 * ȡ��level�����ֶ���
	 */
	virtual const tstring& getField( int level ) const = 0;

	/**
	 * ȡ�õ�ǰ�����ֵ��м���Ϊkey��ֵ
	 * @remarks ע��ȡ����ʱ���׳�NotFindException�쳣
	 */
	virtual const tstring& operator[](const tstring& key) const = 0;

	/**
	 * ��ǰ�������л����ֶ���һ������
	 * @param[ in ] val ��� val Ϊtrue�����õ�ǰ������,����������
	 */
	virtual void isSequenue(bool val) = 0;

	/**
	 * ��ǰ�������л����ֶ���һ������
	 * @return �������true��ǰ������,����������
	 */
	virtual bool isSequenue( ) const = 0;

	/**
	 * ��ǰ�������л����ֶ����ַ���
	 * @param[ in ] val ��� val Ϊtrue�����õ�ǰ���ַ���,�������ַ���
	 * @remarks �ַ���Ҳ����Ϊ��һ������
	 */
	virtual void isString( bool val) = 0;

	/**
	 * ��ǰ�������л����ֶ����ַ���
	 * @return �������true��ǰ���ַ���,�������ַ���
	 * @remarks �ַ���Ҳ����Ϊ��һ������
	 */
	virtual bool isString( ) const = 0;

	/**
	 * ��ǰ�������л����ֶ����ֵ�
	 * @param[ in ] val ���valΪtrue�����õ�ǰ���ֵ�,�������ֵ�
	 * @remarks �ַ���Ҳ����Ϊ��һ������
	 */
	virtual void isDictinary( bool val) = 0;

	/**
	 * ��ǰ�������л����ֶ����ֵ�
	 * @return �������true��ǰ���ֵ�,�������ֵ�
	 * @remarks �ַ���Ҳ����Ϊ��һ������
	 */
	virtual bool isDictinary( ) const = 0;
};

_jingxian_end

#endif // _serialize_context_h_