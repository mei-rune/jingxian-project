/***************************************************************
 * Name:      VirtualFileSystem.cpp
 * Purpose:   Code for VirtualFileSystem Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-11-14
 * Copyright: runner.mei
 * License:
 **************************************************************/

#include "pre_config.h"
#include "VirtualFileSystem.h"


VirtualFileSystem VirtualFileSystem::_virtualFS;

VirtualFileSystem::VirtualFileSystem(void)
: _baseDirectory( wxT( "." ) )
, _download_groups( _baseDirectory, wxT("group.config") )
, _download_tasks( _baseDirectory, wxT("tasks.config") )
, _windows_theme( _baseDirectory, wxT("theme.config") )
, _other( _baseDirectory, wxT("other.config") )
{
}

VirtualFileSystem::~VirtualFileSystem(void)
{
}


VirtualFileSystem& VirtualFileSystem::Get()
{
	return _virtualFS;
}
	
const wxFileName& VirtualFileSystem::GetConfig(SpecialPath path) const
{
	switch( path )
	{
	case DOWNLOAD_GROUPS:
		return _download_groups;
	case DOWNLOAD_TASKS:
		return _download_tasks;
	case WINDOWS_THEME:
		return _windows_theme;
	}
	wxFAIL_MSG( _("no such file!") );
	return _other;
}

	
void VirtualFileSystem::SetBaseDirectory( const wxString& dir)
{
	_baseDirectory = dir;
	
	_download_groups = wxFileName( dir, wxT("group.config") );
	_download_tasks= wxFileName( dir, wxT("tasks.config") );
	_windows_theme= wxFileName( dir, wxT("theme.config") );
	_other = wxFileName( dir, wxT("other.config") );
}