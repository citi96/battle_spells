using Battle_Spells.Api.Singletons.Interfaces;
using System.Collections.Concurrent;

namespace Battle_Spells.Api.Singletons
{
    public class PlayerConnectionTracker : IPlayerConnectionTracker
    {
        private readonly ConcurrentDictionary<Guid, string> _player2cid = new();
        private readonly ConcurrentDictionary<string, Guid> _cid2player = new();

        public bool TryGetConnectionId(Guid pid, out string? cid)
            => _player2cid.TryGetValue(pid, out cid);

        public void Register(Guid pid, string cid)
        {
            _player2cid[pid] = cid;
            _cid2player[cid] = pid;
        }

        public void Unregister(string cid)
        {
            if (_cid2player.TryRemove(cid, out var pid))
                _player2cid.TryRemove(pid, out _);
        }
    }
}
