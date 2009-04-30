/***************************************************************
 * Name:      VirtualFileSystem.h
 * Purpose:   Defines VirtualFileSystem Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-11-14
 * Copyright: runner.mei
 * License:
 **************************************************************/

#ifndef VIRTUALFILESYSTEM_H
#define VIRTUALFILESYSTEM_H

#include "wx/filename.h"

class VirtualFileSystem
{
public:

	enum SpecialPath
	{
		 DOWNLOAD_GROUPS
		,DOWNLOAD_TASKS
		,WINDOWS_THEME
	};

	~VirtualFileSystem(void);

	static VirtualFileSystem& Get();

	const wxFileName& GetConfig( SpecialPath path ) const;

	friend class jxApp;
private:
	
	VirtualFileSystem(void);

	void SetBaseDirectory( const wxString& dir);
	
	wxString _baseDirectory;

	wxFileName _download_groups;
	wxFileName _download_tasks;
	wxFileName _windows_theme;
	wxFileName _other;
	
	static VirtualFileSystem _virtualFS;
};


#endif // VIRTUALFILESYSTEM_H