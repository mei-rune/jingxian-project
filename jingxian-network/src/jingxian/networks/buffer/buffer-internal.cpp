
# include "pro_config.h"
# include "jingxian/networks/buffer/buffer-internal.h"
# include "jingxian/networks/buffer/MemoryBufferHandler.h"
# include "jingxian/networks/buffer/FileBufferHandler.h"
# include "jingxian/networks/buffer/PacketBufferHandler.h"

_jingxian_begin

namespace bufferOP
{
static IBufferHandler* buffer_handler_[] =
{
    null_ptr
    , new MemoryBufferHandler()
    , new FileBufferHandler()
    , new PacketBufferHandler()
};


IBufferHandler* GetBufferHandler(int type)
{
    assert(0 < type && type < 4);

    return buffer_handler_[type];
}
}

_jingxian_end
