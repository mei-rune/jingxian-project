for /R %f in (*.cpp;*.c;*.h) do astyle -p -U -c --style=ansi "%f"
