using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.threading
{

    public delegate void Handler();

    public delegate void Handler<T0>(T0 parameter0);

    public delegate void Handler<T0, T1>(T0 parameter0, T1 parameter1);

    public delegate void Handler<T0, T1, T2>(T0 parameter0, T1 parameter1, T2 parameter2);

    public delegate void Handler<T0, T1, T2, T3>(T0 parameter0, T1 parameter1, T2 parameter2, T3 parameter3);

    public delegate void Handler<T0, T1, T2, T3, T4>(T0 parameter0, T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4);


    public delegate R CallHandler<R>();

    public delegate R CallHandler<R, T0>(T0 parameter0);

    public delegate R CallHandler<R, T0, T1>(T0 parameter0, T1 parameter1);

    public delegate R CallHandler<R, T0, T1, T2>(T0 parameter0, T1 parameter1, T2 parameter2);

    public delegate R CallHandler<R, T0, T1, T2, T3>(T0 parameter0, T1 parameter1, T2 parameter2, T3 parameter3);

    public delegate R CallHandler<R, T0, T1, T2, T3, T4>(T0 parameter0, T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4);


    public delegate void ThrowError(Exception e);

}
