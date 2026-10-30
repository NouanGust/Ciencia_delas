using UnityEngine;
using UnityEngine.InputSystem;

namespace CienciaDelas
{
    public sealed class EntradaDiretor : SingletonComponente<EntradaDiretor>
    {
        private float proximaNavegacao;
        void Update()
        {
            var ui=InterfaceDiretor.Instance; if(ui==null) return;
            var k=Keyboard.current; var m=Mouse.current;
            if(k!=null)
            {
                if(k.escapeKey.wasPressedThisFrame) ui.Pausa();
                if(k.tabKey.wasPressedThisFrame) ui.Informacoes();
                if(k.enterKey.wasPressedThisFrame || k.zKey.wasPressedThisFrame) ui.Acionar();
                if(k.xKey.wasPressedThisFrame || k.backspaceKey.wasPressedThisFrame) ui.Cancelar();
                if(k.leftArrowKey.wasPressedThisFrame || k.aKey.wasPressedThisFrame || k.upArrowKey.wasPressedThisFrame || k.wKey.wasPressedThisFrame) ui.Navegar(-1);
                if(k.rightArrowKey.wasPressedThisFrame || k.dKey.wasPressedThisFrame || k.downArrowKey.wasPressedThisFrame || k.sKey.wasPressedThisFrame) ui.Navegar(1);
            }
            if(m!=null && m.rightButton.wasPressedThisFrame) ui.Cancelar();
            foreach(var g in Gamepad.all)
            {
                if(g.startButton.wasPressedThisFrame) ui.Pausa();
                if(g.buttonSouth.wasPressedThisFrame) ui.Acionar();
                if(g.buttonEast.wasPressedThisFrame) ui.Cancelar();
                if(g.leftShoulder.wasPressedThisFrame) ui.Especial();
                if(g.rightShoulder.wasPressedThisFrame || g.rightTrigger.wasPressedThisFrame) ui.Informacoes();
                var v=g.dpad.ReadValue()+g.leftStick.ReadValue();
                if(v.sqrMagnitude<.25f) continue;
                if(Time.unscaledTime<proximaNavegacao) continue;
                ui.Navegar(v.x<-.4f || v.y>.4f?-1:1); proximaNavegacao=Time.unscaledTime+.22f;
            }
        }
    }
}
