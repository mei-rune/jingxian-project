
#ifndef _configure_h_
#define _configure_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <stack>
# include <list>
# include "jingxian/connection_functionals.h"
# include "jingxian/logging/logging.h"

_jingxian_begin

namespace configure
{

class Context;

typedef _connection_base<bool (Context& context, const tstring& txt)> callback_type;

class IContext
{
public:
  virtual ~IContext() {}

  virtual void connect(callback_type* connection) = 0;

  virtual void disconnect(callback_type* connection) = 0;

  virtual void push(callback_type* callback) = 0;

  virtual void pop() = 0;

  virtual void exit() = 0;

  virtual logging::logger& logger() = 0;
};

class Context : public IContext
{
public:

  Context(IContext* context)
      : pimpl_(context)
  {
  }

  virtual ~Context() {}

  virtual void connect(callback_type* connection)
  {
    pimpl_->connect(connection);
  }

  template<class desttype>
  void connect(desttype* pclass, bool (desttype::*pmemfun)(Context& , const tstring&))
  {
    _connection<desttype, bool (Context& , const tstring&)>* conn =
      new _connection<desttype, bool (Context& , const tstring&)>(pclass, pmemfun);

    connect(conn);
  }

  virtual void disconnect(callback_type* connection)
  {
    pimpl_->disconnect(connection);
  }

  virtual void push(callback_type* connection)
  {
    pimpl_->push(connection);
  }

  template<class desttype>
  void push(desttype* pclass, bool (desttype::*pmemfun)(Context& , const tstring&))
  {
    _connection<desttype, bool (Context& , const tstring&)>* conn =
      new _connection<desttype, bool (Context& , const tstring&)>(pclass, pmemfun);

    push(conn);
  }

  virtual void pop()
  {
    pimpl_->pop();
  }

  virtual void exit()
  {
    pimpl_->exit();
  }

  virtual logging::logger& logger()
  {
	return pimpl_->logger();
  }

private:
  NOCOPY(Context);
  IContext* pimpl_;
};



class ContextImpl : public IContext
{
public:

  class ConnectionSlot
  {
  public:

    typedef std::list<callback_type *>  connections_list;

    ConnectionSlot()
    {
    }

    virtual ~ConnectionSlot()
    {
      for (connections_list::iterator it = m_slots.begin()
                                           ; it != m_slots.end(); ++ it)
        {
          delete (*it);
        }

      m_slots.clear();
    }

    virtual void connect(callback_type* connection)
    {
      m_slots.push_back(connection);
    }

    virtual void disconnect(callback_type* connection)
    {
      m_slots.remove(connection);
    }

    virtual bool call(Context& context, const tstring& txt)
    {
      connections_list::const_iterator itNext, it = m_slots.begin();
      connections_list::const_iterator itEnd = m_slots.end();

      while (it != itEnd)
        {
          itNext = it;
          ++itNext;

          if ((*it)->call(context, txt))
            return true;

          it = itNext;
        }
      return false;
    }
  private:
    NOCOPY(ConnectionSlot);
    connections_list m_slots;
  };

  ContextImpl(logging::logger& alogger)
	  : m_exit(false)
	  , m_logger(alogger)
  {
    m_slots.push(new ConnectionSlot());
  }

  virtual ~ContextImpl() {}

  virtual void connect(callback_type* connection)
  {
    m_slots.top()->connect(connection);
  }

  virtual void disconnect(callback_type* connection)
  {
    m_slots.top()->disconnect(connection);
  }

  virtual void push(callback_type* connection)
  {
    m_slots.push(new ConnectionSlot());
    m_slots.top()->connect(connection);
  }

  virtual void pop()
  {
    m_slots.pop();
  }

  virtual bool call(Context& context, const tstring& txt)
  {
    return m_slots.top()->call(context, txt);
  }

  virtual void exit()
  {
	  m_exit = true;
  }

  virtual bool isExit()
  {
	  return m_exit;
  }

  virtual logging::logger& logger()
  {
	  return m_logger;
  }

private:
  NOCOPY(ContextImpl);
  std::stack<ConnectionSlot*> m_slots;
  bool m_exit;
  logging::logger& m_logger;
};

}

_jingxian_end

#endif //_protocolcontext_h_