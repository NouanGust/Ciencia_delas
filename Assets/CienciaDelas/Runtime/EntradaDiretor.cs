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
            var m=Mouse.current;
            ProcessarTeclado(Keyboard.current);
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

        internal void ProcessarTeclado(Keyboard k)
        {
            var ui = InterfaceDiretor.Instance;
            if (k == null || ui == null) return;
            if (k.escapeKey.wasPressedThisFrame) { ui.Pausa(); return; }
            if (k.tabKey.wasPressedThisFrame) { ui.Informacoes(); return; }
            if (ui.MesaAberta)
            {
                // Each player's navigation and actions use their own remembered focus.
                if (k.aKey.wasPressedThisFrame) ui.NavegarJogador(0, -1);
                if (k.dKey.wasPressedThisFrame) ui.NavegarJogador(0, 1);
                if (k.leftArrowKey.wasPressedThisFrame) ui.NavegarJogador(1, -1);
                if (k.rightArrowKey.wasPressedThisFrame) ui.NavegarJogador(1, 1);
                if (k.zKey.wasPressedThisFrame) ui.AcionarJogador(0);
                if (k.enterKey.wasPressedThisFrame) ui.AcionarJogador(1);
                if (k.xKey.wasPressedThisFrame) ui.CancelarJogador(0);
                if (k.backspaceKey.wasPressedThisFrame) ui.CancelarJogador(1);
                if (k.qKey.wasPressedThisFrame) ui.NavegarAmeacas(-1);
                if (k.eKey.wasPressedThisFrame) ui.NavegarAmeacas(1);
                return;
            }
            if (k.enterKey.wasPressedThisFrame || k.zKey.wasPressedThisFrame) ui.Acionar();
            if (k.xKey.wasPressedThisFrame || k.backspaceKey.wasPressedThisFrame) ui.Cancelar();
            if (k.leftArrowKey.wasPressedThisFrame || k.aKey.wasPressedThisFrame || k.upArrowKey.wasPressedThisFrame || k.wKey.wasPressedThisFrame) ui.Navegar(-1);
            if (k.rightArrowKey.wasPressedThisFrame || k.dKey.wasPressedThisFrame || k.downArrowKey.wasPressedThisFrame || k.sKey.wasPressedThisFrame) ui.Navegar(1);
        }
    }
}
