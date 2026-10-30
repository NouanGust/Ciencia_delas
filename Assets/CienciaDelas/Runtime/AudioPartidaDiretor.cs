using UnityEngine;

namespace CienciaDelas
{
    public sealed class AudioPartidaDiretor : SingletonComponente<AudioPartidaDiretor>
    {
        private ConfiguracaoJogo config;
        private AudioSource musica, efeitos, ambiente;
        private AudioClip desejada;
        public float VolumeMusica { get; private set; }
        public float VolumeEfeitos { get; private set; }
        public void Inicializar(ConfiguracaoJogo c)
        {
            config=c;
            musica=gameObject.AddComponent<AudioSource>(); musica.loop=true; musica.playOnAwake=false;
            efeitos=gameObject.AddComponent<AudioSource>(); efeitos.playOnAwake=false;
            ambiente=gameObject.AddComponent<AudioSource>(); ambiente.loop=true; ambiente.playOnAwake=false; ambiente.clip=c.ambiente;
            VolumeMusica=PlayerPrefs.GetFloat("CienciaDelas.Musica",.65f); VolumeEfeitos=PlayerPrefs.GetFloat("CienciaDelas.Efeitos",.8f);
            PartidaDiretor.Instance.Evento+=Evento;
        }
        public void Ajustar(bool music,float delta)
        {
            if(music) VolumeMusica=Mathf.Clamp01(VolumeMusica+delta); else VolumeEfeitos=Mathf.Clamp01(VolumeEfeitos+delta);
            PlayerPrefs.SetFloat("CienciaDelas.Musica",VolumeMusica); PlayerPrefs.SetFloat("CienciaDelas.Efeitos",VolumeEfeitos); PlayerPrefs.Save();
        }
        public void Som(bool valido=true) { Tocar(valido?config.selecionar:config.cancelar); }
        void Tocar(AudioClip clip) { if(clip!=null) efeitos.PlayOneShot(clip,VolumeEfeitos); }
        void Evento(string e)
        {
            switch(e) { case "compra": Tocar(config.comprar); break; case "especial": Tocar(config.especial); break; case "confirmar": Tocar(config.confirmar); break; case "erro": Tocar(config.cancelar); break; case "carta": Tocar(config.selecionar); break; }
        }
        void Update()
        {
            if(config==null) return;
            var p=PartidaDiretor.Instance;
            desejada=p.Fase==Fase.Menu?config.menu:p.Fase==Fase.Vitoria?config.vitoria:p.Fase==Fase.Derrota?config.derrota:AmeacasDiretor.Instance.Ativas.Count>=3?config.crise:config.partida;
            if(musica.clip!=desejada)
            {
                musica.volume=Mathf.MoveTowards(musica.volume,0,Time.unscaledDeltaTime*2);
                if(musica.volume<=0) { musica.Stop(); musica.clip=desejada; if(desejada!=null) musica.Play(); }
            }
            else musica.volume=Mathf.MoveTowards(musica.volume,VolumeMusica*(p.Pausada?.4f:1),Time.unscaledDeltaTime*2);
            ambiente.volume=VolumeEfeitos*.2f;
            bool tocar=p.Fase!=Fase.Menu && !p.Terminou && !p.Pausada;
            if(tocar && ambiente.clip!=null && !ambiente.isPlaying) ambiente.Play();
            if(!tocar && ambiente.isPlaying) ambiente.Pause();
        }
        protected override void OnDestroy() { PartidaDiretor.Instance.Evento-=Evento; base.OnDestroy(); }
    }
}
