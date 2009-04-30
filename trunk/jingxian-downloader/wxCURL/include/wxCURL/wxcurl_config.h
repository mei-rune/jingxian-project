#ifndef WXCURL_CONFIG_H
#define WXCURL_CONFIG_H

#ifndef __WXMSW__
#define __WXMSW__
#endif

#ifdef _DEBUG
#ifndef __WXDEBUG__
#define __WXDEBUG__ // Add this for debug builds that link to wxWidgets debug configurations. Activates wxASSERT(), etc. See the Debugging Overview for more information.
#endif
#endif

#define _CRT_SECURE_NO_DEPRECATE // Add this if you want to suppress "This function or variable may be unsafe. Consider using <alternative> instead." warnings.
#define _CRT_NONSTDC_NO_DEPRECATE // Suppresses other warnings.

#ifndef CURL_STATICLIB
#define CURL_STATICLIB
#endif

#include <winsock2.h>
#include <ws2tcpip.h>

// basic wxWidgets headers
#include <wx/wxprec.h>

#ifndef WX_PRECOMP
#include "wx/wx.h"
#endif

#include <wx/stream.h>
#include <wx/mstream.h>
#include <wx/wfstream.h>
#include <wx/txtstrm.h>
#include <wx/event.h>
#include <wx/string.h>
#include <wx/datetime.h>
#include <wx/xml/xml.h>

#include <stdio.h>
#include <stdarg.h>
#include <time.h>

#endif // WXCURL_CONFIG_H
