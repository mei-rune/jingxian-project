for /R %f in (*.cpp;*.c;*.h) do astyle --style=gnu "%f"
