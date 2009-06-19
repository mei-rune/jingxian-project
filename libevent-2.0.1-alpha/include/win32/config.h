
#ifndef _PRO_CONFIG_H_
#define _PRO_CONFIG_H_

/* config.h.  Generated from config.h.in by configure.  */
/* config.h.in.  Generated from configure.in by autoheader.  */

/* Define if libevent should not allow replacing the mm functions */
/* #undef _EVENT_DISABLE_MM_REPLACEMENT */

/* Define if libevent should not be compiled with thread support */
/* #undef _EVENT_DISABLE_THREAD_SUPPORT */

/* Define if clock_gettime is available in libc */
/* #undef _EVENT_DNS_USE_CPU_CLOCK_FOR_ID */

/* Define is no secure id variant is available */
#define _EVENT_DNS_USE_GETTIMEOFDAY_FOR_ID 1

/* Define to 1 if you have the <arpa/inet.h> header file. */
/* #undef _EVENT_HAVE_ARPA_INET_H */

/* Define to 1 if you have the `clock_gettime' function. */
/* #undef _EVENT_HAVE_CLOCK_GETTIME */

/* Define if /dev/poll is available */
/* #undef _EVENT_HAVE_DEVPOLL */

/* Define to 1 if you have the <dlfcn.h> header file. */
#define _EVENT_HAVE_DLFCN_H 1

/* Define if your system supports the epoll system calls */
/* #undef _EVENT_HAVE_EPOLL */

/* Define to 1 if you have the `epoll_ctl' function. */
/* #undef _EVENT_HAVE_EPOLL_CTL */

/* Define to 1 if you have the `eventfd' function. */
/* #undef _EVENT_HAVE_EVENTFD */

/* Define if your system supports event ports */
/* #undef _EVENT_HAVE_EVENT_PORTS */

/* Define to 1 if you have the `fcntl' function. */
#define _EVENT_HAVE_FCNTL 1

/* Define to 1 if you have the <fcntl.h> header file. */
#define _EVENT_HAVE_FCNTL_H 1

/* Define to 1 if you have the `getaddrinfo' function. */
#define _EVENT_HAVE_GETADDRINFO 1

/* Define to 1 if you have the `getnameinfo' function. */
#define _EVENT_HAVE_GETNAMEINFO 1

/* Define to 1 if you have the `gettimeofday' function. */
/* #undef _EVENT_HAVE_GETTIMEOFDAY */

/* Define to 1 if you have the `inet_aton' function. */
#define _EVENT_HAVE_INET_ATON 1

/* Define to 1 if you have the `inet_ntop' function. */
#define _EVENT_HAVE_INET_NTOP 1

/* Define to 1 if you have the `inet_pton' function. */
#define _EVENT_HAVE_INET_PTON 1

/* Define to 1 if you have the <inttypes.h> header file. */
/* #undef _EVENT_HAVE_INTTYPES_H */

/* Define to 1 if you have the `kqueue' function. */
#define _EVENT_HAVE_KQUEUE 1

/* Define if the system has zlib */
#define _EVENT_HAVE_LIBZ 1

/* Define to 1 if you have the <memory.h> header file. */
#define _EVENT_HAVE_MEMORY_H 1

/* Define to 1 if you have the `mmap' function. */
/* #undef _EVENT_HAVE_MMAP */

/* Define to 1 if you have the <netinet/in6.h> header file. */
/* #undef _EVENT_HAVE_NETINET_IN6_H */

/* Define to 1 if you have the <netinet/in.h> header file. */
/* #undef  _EVENT_HAVE_NETINET_IN_H */

/* Define to 1 if you have the `pipe' function. */
/* #undef _EVENT_HAVE_PIPE */

/* Define to 1 if you have the `poll' function. */
/* #undef _EVENT_HAVE_POLL */

/* Define to 1 if you have the <poll.h> header file. */
/* #undef _EVENT_HAVE_POLL_H */

/* Define to 1 if you have the `port_create' function. */
/* #undef _EVENT_HAVE_PORT_CREATE */

/* Define to 1 if you have the <port.h> header file. */
/* #undef _EVENT_HAVE_PORT_H */

/* Define if you have POSIX threads libraries and header files. */
/* #undef _EVENT_HAVE_PTHREAD */

/* Define if we have pthreads on this system */
#define _EVENT_HAVE_PTHREADS 1

/* Define to 1 if the system has the type `sa_family_t'. */
#define _EVENT_HAVE_SA_FAMILY_T 1

/* Define to 1 if you have the `select' function. */
/* #undef _EVENT_HAVE_SELECT */

/* Define to 1 if you have the `sendfile' function. */
#define _EVENT_HAVE_SENDFILE 1

/* Define if F_SETFD is defined in <fcntl.h> */
/* #undef _EVENT_HAVE_SETFD */

/* Define to 1 if you have the `sigaction' function. */
/* #undef _EVENT_HAVE_SIGACTION */

/* Define to 1 if you have the `signal' function. */
#define _EVENT_HAVE_SIGNAL 1

/* Define to 1 if you have the <signal.h> header file. */
#define _EVENT_HAVE_SIGNAL_H 1

/* Define to 1 if you have the `splice' function. */
/* #undef _EVENT_HAVE_SPLICE */

/* Define to 1 if you have the <stdarg.h> header file. */
#define _EVENT_HAVE_STDARG_H 1

/* Define to 1 if you have the <stddef.h> header file. */
#define _EVENT_HAVE_STDDEF_H 1

/* Define to 1 if you have the <stdint.h> header file. */
/* #undef _EVENT_HAVE_STDINT_H */

/* Define to 1 if you have the <stdlib.h> header file. */
#define _EVENT_HAVE_STDLIB_H 1

/* Define to 1 if you have the <strings.h> header file. */
#define _EVENT_HAVE_STRINGS_H 1

/* Define to 1 if you have the <string.h> header file. */
#define _EVENT_HAVE_STRING_H 1

/* Define to 1 if you have the `strlcpy' function. */
/* #undef _EVENT_HAVE_STRLCPY */

/* Define to 1 if you have the `strsep' function. */
#define _EVENT_HAVE_STRSEP 1

/* Define to 1 if you have the `strtok_r' function. */
#define _EVENT_HAVE_STRTOK_R 1

/* Define to 1 if you have the `strtoll' function. */
/* #undef _EVENT_HAVE_STRTOLL */

/* Define to 1 if the system has the type `struct in6_addr'. */
#define _EVENT_HAVE_STRUCT_IN6_ADDR 1

/* Define to 1 if `s6_addr16' is member of `struct in6_addr'. */
/* #undef _EVENT_HAVE_STRUCT_IN6_ADDR_S6_ADDR16 */

/* Define to 1 if `s6_addr32' is member of `struct in6_addr'. */
/* #undef _EVENT_HAVE_STRUCT_IN6_ADDR_S6_ADDR32 */

/* Define to 1 if the system has the type `struct sockaddr_in6'. */
#define _EVENT_HAVE_STRUCT_SOCKADDR_IN6 1

/* Define to 1 if `sin6_len' is member of `struct sockaddr_in6'. */
/* #undef _EVENT_HAVE_STRUCT_SOCKADDR_IN6_SIN6_LEN *

/* Define to 1 if `sin_len' is member of `struct sockaddr_in'. */
/* #undef _EVENT_HAVE_STRUCT_SOCKADDR_IN_SIN_LEN */

/* Define to 1 if you have the <sys/devpoll.h> header file. */
/* #undef _EVENT_HAVE_SYS_DEVPOLL_H */

/* Define to 1 if you have the <sys/epoll.h> header file. */
/* #undef _EVENT_HAVE_SYS_EPOLL_H */

/* Define to 1 if you have the <sys/eventfd.h> header file. */
/* #undef _EVENT_HAVE_SYS_EVENTFD_H */

/* Define to 1 if you have the <sys/event.h> header file. */
#define _EVENT_HAVE_SYS_EVENT_H 1

/* Define to 1 if you have the <sys/ioctl.h> header file. */
/* #undef _EVENT_HAVE_SYS_IOCTL_H */

/* Define to 1 if you have the <sys/mman.h> header file. */
/* #undef _EVENT_HAVE_SYS_MMAN_H */

/* Define to 1 if you have the <sys/param.h> header file. */
/* #undef _EVENT_HAVE_SYS_PARAM_H */

/* Define to 1 if you have the <sys/queue.h> header file. */
/* #undef _EVENT_HAVE_SYS_QUEUE_H */

/* Define to 1 if you have the <sys/select.h> header file. */
/* #undef _EVENT_HAVE_SYS_SELECT_H */

/* Define to 1 if you have the <sys/sendfile.h> header file. */
/* #undef _EVENT_HAVE_SYS_SENDFILE_H */

/* Define to 1 if you have the <sys/socket.h> header file. */
/* #undef _EVENT_HAVE_SYS_SOCKET_H */

/* Define to 1 if you have the <sys/stat.h> header file. */
#define _EVENT_HAVE_SYS_STAT_H 1

/* Define to 1 if you have the <sys/time.h> header file. */
/* #undef _EVENT_HAVE_SYS_TIME_H */

/* Define to 1 if you have the <sys/types.h> header file. */
#define _EVENT_HAVE_SYS_TYPES_H 1

/* Define to 1 if you have the <sys/uio.h> header file. */
/* #undef _EVENT_HAVE_SYS_UIO_H */

/* Define if TAILQ_FOREACH is defined in <sys/queue.h> */
/* #undef _EVENT_HAVE_TAILQFOREACH */

/* Define if timeradd is defined in <sys/time.h> */
/* #undef _EVENT_HAVE_TIMERADD */

/* Define if timerclear is defined in <sys/time.h> */
/* #undef _EVENT_HAVE_TIMERCLEAR */

/* Define if timercmp is defined in <sys/time.h> */
/* #undef _EVENT_HAVE_TIMERCMP */

/* Define if timerisset is defined in <sys/time.h> */
/* #undef _EVENT_HAVE_TIMERISSET */

/* Define to 1 if the system has the type `uint16_t'. */
/* #undef _EVENT_HAVE_UINT16_T *

/* Define to 1 if the system has the type `uint32_t'. */
/* #undef _EVENT_HAVE_UINT32_T */

/* Define to 1 if the system has the type `uint64_t'. */
/* #undef _EVENT_HAVE_UINT64_T */

/* Define to 1 if the system has the type `uint8_t'. */
/* #undef _EVENT_HAVE_UINT8_T *

/* Define to 1 if you have the <unistd.h> header file. */
/* #undef _EVENT_HAVE_UNISTD_H */

/* Define to 1 if you have the `vasprintf' function. */
#define _EVENT_HAVE_VASPRINTF 1

/* Define if kqueue works correctly with pipes */
/* #undef _EVENT_HAVE_WORKING_KQUEUE */

/* Numeric representation of the version */
#define _EVENT_NUMERIC_VERSION 0x02000100

/* Name of package */
#define _EVENT_PACKAGE "libevent"

/* Define to the address where bug reports for this package should be sent. */
#define _EVENT_PACKAGE_BUGREPORT ""

/* Define to the full name of this package. */
#define _EVENT_PACKAGE_NAME ""

/* Define to the full name and version of this package. */
#define _EVENT_PACKAGE_STRING ""

/* Define to the one symbol short name of this package. */
#define _EVENT_PACKAGE_TARNAME ""

/* Define to the version of this package. */
#define _EVENT_PACKAGE_VERSION ""

/* Define to necessary symbol if this constant uses a non-standard name on
   your system. */
/* #undef _EVENT_PTHREAD_CREATE_JOINABLE */

/* The size of `int', as computed by sizeof. */
#define _EVENT_SIZEOF_INT 4

/* The size of `long', as computed by sizeof. */
#define _EVENT_SIZEOF_LONG 4

/* The size of `long long', as computed by sizeof. */
#define _EVENT_SIZEOF_LONG_LONG 8

/* The size of `short', as computed by sizeof. */
#define _EVENT_SIZEOF_SHORT 2

/* Define to 1 if you have the ANSI C header files. */
#define _EVENT_STDC_HEADERS 1

/* Define to 1 if you can safely include both <sys/time.h> and <time.h>. */
#define _EVENT_TIME_WITH_SYS_TIME 1

/* Version number of package */
#define _EVENT_VERSION "2.0.1-alpha"

/* Define to appropriate substitue if compiler doesnt have __func__ */
/* #undef _EVENT___func__ */

/* Define to empty if `const' does not conform to ANSI C. */
/* #undef _EVENT_const */

/* Define to `__inline__' or `__inline' if that's what the C compiler
   calls it, or to nothing if 'inline' is not supported under any name.  */
#ifndef _EVENT___cplusplus
/* #undef _EVENT_inline */
#endif

/* Define to `int' if <sys/types.h> does not define. */
/* #undef _EVENT_pid_t */

/* Define to `unsigned int' if <sys/types.h> does not define. */
/* #undef _EVENT_size_t */

/* Define to unsigned int if you dont have it */
/* #undef _EVENT_socklen_t */

/* config.h.  Generated by configure.  */
/* config.h.in.  Generated from configure.in by autoheader.  */

/* Define if clock_gettime is available in libc */
/* #undef DNS_USE_CPU_CLOCK_FOR_ID */

/* Define if no secure id variant is available */
/* #undef DNS_USE_FTIME_FOR_ID */

/* Define if no secure id variant is available */
/* #define DNS_USE_GETTIMEOFDAY_FOR_ID 1 */

/* Define to 1 if you have the `clock_gettime' function. */
/* #undef HAVE_CLOCK_GETTIME */

/* Define if /dev/poll is available */
/* #undef HAVE_DEVPOLL */

/* Define to 1 if you have the <dlfcn.h> header file. */
/* #undef HAVE_DLFCN_H */

/* Define if your system supports the epoll system calls */
/* #undef HAVE_EPOLL */

/* Define to 1 if you have the `epoll_ctl' function. */
/* #undef HAVE_EPOLL_CTL */

/* Define if your system supports event ports */
/* #undef HAVE_EVENT_PORTS */

/* Define to 1 if you have the `fcntl' function. */
/* #undef HAVE_FCNTL */

/* Define to 1 if you have the <fcntl.h> header file. */
#define HAVE_FCNTL_H 1

/* Define to 1 if you have the `getaddrinfo' function. */
/* #undef HAVE_GETADDRINFO */

/* Define to 1 if you have the `getnameinfo' function. */
/* #undef HAVE_GETNAMEINFO */

/* Define to 1 if you have the `gettimeofday' function. */
/* #define HAVE_GETTIMEOFDAY 1 */

/* Define to 1 if you have the `inet_ntop' function. */
/* #undef HAVE_INET_NTOP */

/* Define to 1 if you have the <inttypes.h> header file. */
/* #undef HAVE_INTTYPES_H 1 */

/* Define to 1 if you have the `kqueue' function. */
/* #undef HAVE_KQUEUE */

/* Define to 1 if you have the `nsl' library (-lnsl). */
/* #undef HAVE_LIBNSL */

/* Define to 1 if you have the `resolv' library (-lresolv). */
/* #undef HAVE_LIBRESOLV */

/* Define to 1 if you have the `rt' library (-lrt). */
/* #undef HAVE_LIBRT */

/* Define to 1 if you have the `socket' library (-lsocket). */
/* #undef HAVE_LIBSOCKET */

/* Define to 1 if you have the <memory.h> header file. */
#define HAVE_MEMORY_H 1

/* Define to 1 if you have the <netinet/in6.h> header file. */
/* #undef HAVE_NETINET_IN6_H */

/* Define to 1 if you have the `poll' function. */
/* #undef HAVE_POLL */

/* Define to 1 if you have the <poll.h> header file. */
/* #undef HAVE_POLL_H */

/* Define to 1 if you have the `port_create' function. */
/* #undef HAVE_PORT_CREATE */

/* Define to 1 if you have the <port.h> header file. */
/* #undef HAVE_PORT_H */

/* Define to 1 if you have the `select' function. */
/* #undef HAVE_SELECT */

/* Define if F_SETFD is defined in <fcntl.h> */
/* #undef HAVE_SETFD */

/* Define to 1 if you have the `sigaction' function. */
/* #undef HAVE_SIGACTION */

/* Define to 1 if you have the `signal' function. */
#define HAVE_SIGNAL 1

/* Define to 1 if you have the <signal.h> header file. */
#define HAVE_SIGNAL_H 1

/* Define to 1 if you have the <stdarg.h> header file. */
#define HAVE_STDARG_H 1

/* Define to 1 if you have the <stdint.h> header file. */
/* #define HAVE_STDINT_H 1 */

/* Define to 1 if you have the <stdlib.h> header file. */
#define HAVE_STDLIB_H 1

/* Define to 1 if you have the <strings.h> header file. */
#define HAVE_STRINGS_H 1

/* Define to 1 if you have the <string.h> header file. */
#define HAVE_STRING_H 1

/* Define to 1 if you have the `strlcpy' function. */
#define HAVE_STRLCPY 1

/* Define to 1 if you have the `strsep' function. */
/* #undef HAVE_STRSEP */

/* Define to 1 if you have the `strtok_r' function. */
/* #undef HAVE_STRTOK_R */

/* Define to 1 if the system has the type `struct in6_addr'. */
#define HAVE_STRUCT_IN6_ADDR 1

/* Define to 1 if you have the <sys/devpoll.h> header file. */
/* #undef HAVE_SYS_DEVPOLL_H */

/* Define to 1 if you have the <sys/epoll.h> header file. */
/* #undef HAVE_SYS_EPOLL_H */

/* Define to 1 if you have the <sys/event.h> header file. */
/* #undef HAVE_SYS_EVENT_H */

/* Define to 1 if you have the <sys/ioctl.h> header file. */
/* #undef HAVE_SYS_IOCTL_H */

/* Define to 1 if you have the <sys/queue.h> header file. */
/* #undef HAVE_SYS_QUEUE_H */

/* Define to 1 if you have the <sys/select.h> header file. */
/* #undef HAVE_SYS_SELECT_H */

/* Define to 1 if you have the <sys/socket.h> header file. */
/* #undef HAVE_SYS_SOCKET_H */

/* Define to 1 if you have the <sys/stat.h> header file. */
/* #define HAVE_SYS_STAT_H 1 */

/* Define to 1 if you have the <sys/time.h> header file. */
/* #define HAVE_SYS_TIME_H 1 */

/* Define to 1 if you have the <sys/types.h> header file. */
/* #define HAVE_SYS_TYPES_H 1 */

/* Define if TAILQ_FOREACH is defined in <sys/queue.h> */
/* #undef HAVE_TAILQFOREACH */

/* Define if timeradd is defined in <sys/time.h> */
/* #undef HAVE_TIMERADD */

/* Define if timerclear is defined in <sys/time.h> */
/* #define HAVE_TIMERCLEAR 1 */

/* Define if timercmp is defined in <sys/time.h> */
#define HAVE_TIMERCMP 1

/* Define if timerisset is defined in <sys/time.h> */
#define HAVE_TIMERISSET 1

/* Define to 1 if you have the <unistd.h> header file. */
/* #define HAVE_UNISTD_H 1 */

/* Define to 1 if you have the `vasprintf' function. */
/* #undef HAVE_VASPRINTF */

/* Define if kqueue works correctly with pipes */
/* #undef HAVE_WORKING_KQUEUE */

/* Name of package */
#define PACKAGE "libevent"

/* Define to the address where bug reports for this package should be sent. */
#define PACKAGE_BUGREPORT ""

/* Define to the full name of this package. */
#define PACKAGE_NAME ""

/* Define to the full name and version of this package. */
#define PACKAGE_STRING ""

/* Define to the one symbol short name of this package. */
#define PACKAGE_TARNAME ""

/* Define to the version of this package. */
#define PACKAGE_VERSION ""

/* Define to 1 if you have the ANSI C header files. */
#define STDC_HEADERS 1

/* Define to 1 if you can safely include both <sys/time.h> and <time.h>. */
#define TIME_WITH_SYS_TIME 1

/* Version number of package */
#define VERSION "1.3.99-trunk"

/* Define to appropriate substitue if compiler doesnt have __func__ */
#if defined(_MSC_VER) && _MSC_VER < 1300
#define __func__ "??"
#else
#define __func__ __FUNCTION__
#endif

/* Define to empty if `const' does not conform to ANSI C. */
/* #undef const */

/* Define to `__inline__' or `__inline' if that's what the C compiler
   calls it, or to nothing if 'inline' is not supported under any name.  */
#ifndef __cplusplus
#define inline __inline
#endif

/* Define to `int' if <sys/types.h> does not define. */
/* #undef pid_t */

/* Define to `unsigned' if <sys/types.h> does not define. */
/* #undef size_t */

/* Define to unsigned int if you dont have it */
/* #undef  socklen_t unsigned int */

/* Define to `unsigned short' if <sys/types.h> does not define. */
#define uint16_t unsigned short

/* Define to `unsigned int' if <sys/types.h> does not define. */
#define uint32_t unsigned int

/* Define to `unsigned long long' if <sys/types.h> does not define. */
#define uint64_t __uint64_t

/* Define to `unsigned char' if <sys/types.h> does not define. */
#define uint8_t unsigned char

#define ssize_t  size_t

#endif
