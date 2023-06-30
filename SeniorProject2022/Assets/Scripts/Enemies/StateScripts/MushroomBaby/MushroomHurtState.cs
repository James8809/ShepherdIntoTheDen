using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Enemy{
    public class MushroomHurtState : IEnemyState
    {
        private MushroomBabyEnemyAgent _mushAgent;
        public EnemyStateId previousState;
        private List<SkinnedMeshRenderer> _mushMeshes;   // must be set in inspector.
        private List<Material> toonMats;    // no more untextured enemies :(

        public MushroomHurtState(MushroomBabyEnemyAgent mushAgent)
        {
            _mushAgent = mushAgent;
            _mushMeshes = new List<SkinnedMeshRenderer>(mushAgent.GetComponentsInChildren<SkinnedMeshRenderer>());
            toonMats = new List<Material>(_mushMeshes.Select(x => x.material));
        }

        public void Enter(EnemyAgent agent)
        {
            agent.navMeshAgent.speed = 0;
            agent.StartCoroutine(HurtSequence());
        }

        private void SetMaterials(Material mat)
        {
            foreach(SkinnedMeshRenderer mesh in _mushMeshes)
            {
                mesh.material = mat;
            }
        }

        private void SetBaseMaterial()
        {
            for(int i = 0; i < _mushMeshes.Count; i++)
            {
                _mushMeshes[i].material = toonMats[i];
            }
        }

        private IEnumerator HurtSequence()
        {
            SetMaterials(_mushAgent.flashMat);
            yield return null;
            SetMaterials(_mushAgent.blackMat);
            yield return null;
            yield return null;
            SetMaterials(_mushAgent.flashMat);
            yield return null;
            yield return null;
            SetMaterials(_mushAgent.blackMat);
            yield return null;
            yield return null;
            yield return null;
            SetMaterials(_mushAgent.flashMat);
            yield return null;
            yield return null;
            SetBaseMaterial();
            yield return new WaitForSeconds(.13f);
            // exit state
            _mushAgent.stateMachine.ChangeState(previousState);
        }

        public void Update(EnemyAgent agent)
        {

        }

        public void Exit(EnemyAgent agent)
        {
            agent.StopCoroutine("HurtSequence"); // easier to use strings than to track \('_')/
        }

        public EnemyStateId GetId()
        {
            return EnemyStateId.Hurt;
        }
    }
}
