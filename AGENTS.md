# AGENTS.md — Guia Operacional para Agentes de IA (`r.e.p.o`)

Documento de referência técnica para agentes de IA que forem ler, modificar ou criar código e assets neste repositório Unity. Baseado exclusivamente na estrutura e nos arquivos reais do projeto.

---

## 1. Visão Geral do Projeto

- **Escopo / Gênero**: Jogo 3D de plataforma e puzzle em terceira pessoa (*r.e.p.o* — trabalho interdisciplinar do 4º semestre de Jogos Digitais da PUC Minas, conforme `README.md`), com mecânica central de **viagem no tempo** entre Futuro e Passado (`TimeTravel`), **parada/desaceleração temporal** de objetos e inimigos (`TimeManager`, `ITimeAffectable`, `PlayerTimeControl`), movimentação com `CharacterController` (pulo, coyote time, jump buffer, ledge grab, slide, dash com pós-imagem `Sandevistan`), manipulação de caixas (`BoxBehaviour`), coleta de chaves/colecionáveis, sistema de diálogos e cards de tutorial/alerta.
- **Versão do Unity**: `6000.3.9f1` (Unity 6 — `ProjectSettings/ProjectVersion.txt`).
- **Pipeline de Renderização**: **URP (Universal Render Pipeline) `17.3.0`**.
  - Perfis em `Assets/Settings/`: `PC_RPAsset.asset` (padrão em `GraphicsSettings.asset` e perfil `PC` no `QualitySettings.asset` para `Standalone`) e `Mobile_RPAsset.asset` (perfil `Mobile`).
- **Principais Pacotes e Bibliotecas (`Packages/manifest.json` e `Assets/`)**:
  - **Renderização & VFX**: `com.unity.render-pipelines.universal` (`17.3.0`), `com.unity.shadergraph` (`17.3.0`), `com.unity.visualeffectgraph` (`17.3.0`), `com.unity.postprocessing` (`3.5.4`).
  - **Input & Câmera**: `com.unity.inputsystem` (`1.18.0` — Unity Input System via `InputAction.CallbackContext`), `com.unity.cinemachine` (`3.1.7` — ex.: `CinemachineInputAxisController`).
  - **UI & Tweening**: `com.unity.ugui` (`2.0.0`), `Assets/TextMesh Pro/` (TMPro) e `Assets/LeanTween/` (animações de UI, glitch, fade, rotação de drone e crossfade de áudio).
  - **Level Design & Navegação**: `com.unity.probuilder` (`6.0.9`), `com.unity.ai.navigation` (`2.0.14`), `com.unity.timeline` (`1.8.12`), `com.unity.2d.animation` (`13.0.6`).
  - **Tooling & Testes**: `com.unity.test-framework` (`1.6.0`), `com.coplaydev.unity-mcp`.
- **Cenas Ativas no Build (`ProjectSettings/EditorBuildSettings.asset`)**:
  - Habilitadas (`enabled: 1`): `Assets/Scenes/Menu/Menu.unity`, `Assets/Scenes/Lobby.unity`, `Assets/Scenes/Level_Armazem.unity`, `Assets/Scenes/museu.unity`.
  - Presentes no build mas desabilitadas (`enabled: 0`): `Assets/Scenes/Level_Predio.unity`, `Assets/Scenes/Level_Sky.unity`.

---

## 2. Estrutura de Pastas e Organização

Todo o código C# do jogo compila no assembly padrão `Assembly-CSharp` (não há `.asmdef` de jogo; o único `.asmdef` em `Assets/` pertence aos exemplos de editor do ProBuilder).

| Caminho | Conteúdo |
|---|---|
| `Assets/Scripts/Managers/` | Singletons e gerenciadores centrais (`GameManager`, `TimeManager`, `TimeTravel`, `MusicManager`, `MenuManager`). |
| `Assets/Scripts/Player/` | Movimentação, dash, vida, interação, controle de tempo, detecção de colisão e efeito `Sandevistan`. |
| `Assets/Scripts/TimeTravel/` | `TimeTravel_ManagerInfo` (referências de mapa/modelo/skybox por era) e `TimeTravel_ObjectBehavior` (alternância futuro/passado por objeto). |
| `Assets/Scripts/Enemies/` | Base `ChasePlayer` e inimigos (`Cockroach`, `Drone`), além de `ContactDamage`. |
| `Assets/Scripts/Puzzles/` | Lógica de puzzles (`BoxBehaviour`, `ItemBehavior`, `CaixasCaindo/`, `PortaPuzzleCaixas/`, `MoveSpawn`). |
| `Assets/Scripts/World/` | Plataformas (`PlatformBehavior`, `PlatormBreakableBehavior`), `Collectibles/` (`Collectible`, `CollectibleManager`, `CollectiblePool`, `ICollectible`), `PlayerCollisions/` (`PlayerCollisionObject`, `PC_CheckpointBehavior`, `PC_KillzoneBehavior`, `PC_PlatformBreakable`). |
| `Assets/Scripts/Dialogs/` | `DialogData` (`ScriptableObject`), `DialogManager`, `TriggerDialog`. |
| `Assets/Scripts/UI/` | `Hud`, `Pause`, `MenuUi`, `UIGlitch`, tutoriais e subpasta `CanvasInfo/` (`InfoManager`, `CardBehavior`, `CardData/` com `CardData.cs` e assets `.asset`). |
| `Assets/Scripts/Utils/` | Utilitários de fase (`BlockTimeTravelArea`, `TransitionSceneTrigger`, `UiCardTrigger`, `EnableObjectOnDestroy`). |
| `Assets/Scripts/Level_Sky/` | Scripts específicos da cena `Level_Sky` (`Interactable`, `PressurePlate`). |
| `Assets/Scenes/` | Cenas do projeto. `Assets/Scenes/_Antigo/` e `Assets/_Recovery/` contêm cenas antigas/de backup. |
| `Assets/Prefab/` | Prefabs organizados por contexto (`Items/`, `Itens Interativos/`, `Puzzle/`, `UI/`, `WorldUtils/`, `_Level_Armazem/`, `_Lobby/`, `Quarteirão/`, `Decoração/`). |
| `Assets/Dialogs/` | Assets `DialogData` (`.asset`) organizados por contexto/fase (`Chaves/`, `DialogDudu/`). |
| `Assets/Settings/` | Assets do URP (`PC_RPAsset`, `PC_Renderer`, `Mobile_RPAsset`, `Mobile_Renderer`, `UniversalRenderPipelineGlobalSettings`) e `VolumeProfile`s. |
| `Assets/Materials/` e `Assets/VFX/` | Materiais, texturas, Shader Graphs (`.shadergraph`), Sub Graphs e VFX Graphs (`.vfx`), além de `Tests_Materials/` e `Provisórios/`. |
| `Assets/Models/` | Modelos `.fbx`, texturas e materiais de personagens (`Alice/`, `Ecilia`), inimigos (`Drones/`) e cenários. Pasta `_SemUso/` guarda assets descartados. |
| `Assets/2dArt/` | Texturas de árvores/props e sprites de HUD (`Hud/`). |
| `Assets/SFX/` | `MasterVolume.mixer`, músicas (`Musicas/`) e efeitos (`SFX/`). |
| `Assets/LeanTween/` e `Assets/TextMesh Pro/` | Pacotes importados em `Assets/` — **não modificar**. |

### Convenções de Nomenclatura Observadas
- **Classes e arquivos C#**: `PascalCase`, um `MonoBehaviour`/`ScriptableObject`/interface por arquivo, sem declaração de `namespace` (escopo global).
- **Prefixos usados no projeto**:
  - `PC_` para subclasses de `PlayerCollisionObject` (`PC_CheckpointBehavior`, `PC_KillzoneBehavior`, `PC_PlatformBreakable`).
  - `TimeTravel_` para scripts do sistema de viagem temporal (`TimeTravel_ManagerInfo`, `TimeTravel_ObjectBehavior`).
  - `I` para interfaces (`ITimeAffectable`, `ICollectible`).
  - `_` em pastas temporárias, específicas de fase ou arquivadas (`_Antigo`, `_SemUso`, `_Recovery`, `_Lobby`, `_Level_Armazem`).
- **Membros C#**: métodos públicos/privados em `PascalCase`; campos e variáveis locais em `camelCase` (com ocorrências pontuais de `_camelCase` em campos privados como `_playerMovement`, `_timeTravelDefaultColor`).

---

## 3. Arquitetura e Padrões C#

### Padrões Existentes
- **Singletons de Cena e Persistentes**:
  - `GameManager` (`[DefaultExecutionOrder(-1)]`, `DontDestroyOnLoad`) e `MusicManager` (`DontDestroyOnLoad`, propriedade estática `instance` em minúsculo) persistem entre cenas.
  - `TimeManager`, `TimeTravel`, `DialogManager`, `InfoManager` e `MenuManager` usam Singleton local da cena (destruindo duplicatas no `Awake`).
  - `CollectibleManager` é uma classe estática em memória (`collected` e `collectedIDs` indexados para áreas `0`, `1` e `2`).
- **Eventos e Desacoplamento**:
  - `TimeTravel.OnTimeChange` (`public static UnityEvent`) notifica objetos dependentes da era atual (`TimeTravel.isFuture`). **Regra crítica**: todo listener registrado em `Start`/`OnEnable` (ex.: `Cockroach`, `Drone`, `PlayerInteraction`) deve remover o listener em `OnDestroy` (`RemoveListener`), pois `OnTimeChange` é `static`. `TimeTravel.OnDestroy` também limpa os listeners quando a instância principal é destruída.
  - `ITimeAffectable` (`float timeMultiplier { get; set; }`) é a interface usada por `TimeManager` para desacelerar/pausar entidades (`ChasePlayer`, `PlatformBehavior`) sem alterar `Time.timeScale`.
  - **Colisões com o Player**: o jogador usa `CharacterController` (que não dispara `OnCollisionEnter` padrão). Objetos que reagem ao contato do player devem herdar de `PlayerCollisionObject` e sobrescrever `OnPlayerCollisionEnter/Stay/Exit` (disparados por `PlayerCollisionSelf`) ou `OnPlayerTriggerEnter/Stay/Exit`.
- **ScriptableObjects**:
  - `DialogData` (`[CreateAssetMenu(..., menuName = "Scriptable Objects/DialogData")]`): nós encadeados de diálogo via `nextDialog[]`.
  - `CardData` (`[CreateAssetMenu(..., menuName = "Scriptable Objects/CardData")]`): título, descrição e duração para cards no `InfoManager`.
- **Input System**:
  - Ações recebem `InputAction.CallbackContext` (ex.: `OnMove`, `OnJump`, `OnDash`, `Interact`, `OnChangeTime`, `PauseGame`, `TimeStop`).
- **Ciclo de Vida e Tempo (`Time.timeScale`)**:
  - `TimeTravel.ChangeTime()` e `Pause.PauseGame()` alteram `Time.timeScale = 0f`.
  - Animações de UI e transições executadas durante pausa ou troca de tempo **precisam** usar `.setIgnoreTimeScale(true)` no `LeanTween` e `WaitForSecondsRealtime` em corrotinas (padrão já adotado em `Hud`, `DialogManager` e `UIGlitch`).
  - Sempre restaurar `Time.timeScale = 1f` ao encerrar transição/pause/morte.

### O Que Evitar ao Editar ou Criar Scripts
- **Não alterar `PlayerCollisionSelf` nem `AdjustMovementForBox` em `PlayerMovement` sem total domínio do fluxo**: ambos contêm lógica sensível de `OverlapCapsuleNonAlloc` / `BoxCastNonAlloc` e avisos explícitos nos comentários para não mexer sem necessidade.
- **Não registrar em `TimeManager.Instance.Register(this)` dentro de `FixedUpdate` em novos scripts**: `ChasePlayer` atualmente faz isso, mas `TimeManager` usa uma `List<ITimeAffectable>` sem checagem de duplicata (`Contains`) e já varre `FindObjectsByType` no `Start` e `OnSceneLoaded`. Em novos scripts `ITimeAffectable`, confie no registro de cena ou registre uma única vez em `Start`/`OnEnable` e faça `Unregister` em `OnDestroy`.
- **Não duplicar Singletons**: ao acessar `GameManager.Instance`, `DialogManager.Instance`, `InfoManager.Instance`, `TimeTravel.Instance`, `TimeManager.Instance` ou `MusicManager.instance`, verifique `!= null` em pontos sujeitos a ordem de inicialização ou troca de cena.
- **Evitar `Time.timeScale = 0` travado**: nunca saia de um fluxo de `Hud.BlackFade` ou `Pause` sem garantir o retorno de `Time.timeScale = 1f` e estado consistente de `Cursor.lockState` / `Cursor.visible`.
- **Não criar `namespace` arbitrário em `Assets/Scripts/` sem alinhar com os scripts existentes**, pois todas as classes atuais estão no namespace global e referenciam-se diretamente (ou via `UnityEngine` / `Unity.Cinemachine` / `TMPro`).
- **Não mexer em `Assets/LeanTween/`** ou `Assets/TextMesh Pro/`.

---

## 4. URP, Física e Assets

### URP, Materiais, Shaders e Pós-Processamento
- **Shaders e Materiais**: o projeto roda em **URP `17.3.0`**. Todos os materiais devem usar shaders URP (`Universal Render Pipeline/Lit`, `Unlit`, ou **Shader Graph** em `Assets/Materials/*.shadergraph` e `Assets/VFX/**/*.shadergraph`). Nunca usar shaders `Standard` do Built-in Render Pipeline.
- **Render Features e Qualidade**: `PC_Renderer.asset` possui `ScreenSpaceAmbientOcclusion` (SSAO) ativo. `PC_RPAsset.asset` habilita Depth Texture, Opaque Texture, HDR, SRP Batcher e Shadow Distance `50` (4 cascatas).
- **Volumes e Troca de Skybox**:
  - `PlayerTimeControl` troca `Volume.sharedProfile` (ex.: `normal` vs. `timeStop`) — prefira `sharedProfile` a instanciar `profile` em runtime para evitar vazamento de material/perfil.
  - `TimeTravel` alterna `RenderSettings.skybox` entre os `TimeTravel_ManagerInfo` do Futuro e do Passado.
- **VFX**: efeitos visuais usam VFX Graph (`.vfx`), `ShaderGraph` e `ParticleSystem` em `Assets/VFX/`.

### Física, Colliders e Layer Collision Matrix
- **Tags e Layers (`TagManager.asset`)**:
  - Tags do projeto: `Player` (padrão), `Platform`, `Enemy`, `GameManager`, `Caixa`.
  - Layers configuradas: `0: Default`, `1: TransparentFX`, `2: Ignore Raycast`, `3: Map`, `4: Water`, `5: UI`, `6: Utils`.
- **`Layer Collision Matrix` (`DynamicsManager.asset`)**:
  - `Default` colide com `Default`, `Map` e `Utils`.
  - `Map` colide **apenas** com `Default`.
  - `Utils` colide **apenas** com `Default`.
  - `TransparentFX`, `Ignore Raycast`, `Water` e `UI` não colidem com nenhuma layer na matriz física.
  - Ao criar triggers, plataformas ou áreas utilitárias (`BlockTimeTravelArea`, `TransitionSceneTrigger`, checkpoints), coloque os objetos em `Default`, `Map` ou `Utils` conforme o comportamento esperado, lembrando que `Map` não colide com `Map` nem com `Utils`.
- **Colliders (`MeshCollider` vs. Primitivos)**:
  - Vários prefabs modulares de cenário já usam `MeshCollider` (ex.: `Quarteirao_*.prefab`, `Passarela*.prefab`, `Escada*.prefab`, `PredioCyberpunk_03.prefab`). Para **novos** props, caixas, inimigos e áreas de trigger, **prefira colliders primitivos** (`BoxCollider`, `CapsuleCollider`, `SphereCollider`) em vez de `MeshCollider` não-convexo ou de alta contagem de polígonos.
  - O player usa `CharacterController` com `AutoSyncTransforms = false` e `SimulationMode = FixedUpdate`. Teleportes devem desabilitar e reabilitar o `CharacterController` (como feito em `PlayerMovement.ExecuteTeleport`).

### Performance e Memória
- **Evitar alocações por frame em `Update`/`FixedUpdate`**:
  - Siga o padrão já usado em `PlayerCollisionSelf` e `PlayerMovement.AdjustMovementForBox`: reutilize buffers pré-alocados com `Physics.OverlapCapsuleNonAlloc`, `Physics.OverlapSphereNonAlloc` e `Physics.BoxCastNonAlloc` (lembrando de limpar referências antigas se necessário).
  - Evite `Physics.OverlapCapsule` com alocação de array em código novo quando houver alternativa `NonAlloc` (nota: `PlayerMovement.HasLedgeLandingClearance` ainda usa `Physics.OverlapCapsule` — prefira não repetir esse padrão).
- **Evitar `FindFirstObjectByType`, `FindObjectOfType`, `GameObject.Find` e `GetComponent` em loops**:
  - Cacheie referências no `Awake` ou `Start` (ex.: `characterController`, `playerMovement`, `animator`, `mainCamera`).
  - `TimeManager` e `GameManager`/`Hud` usam `FindObjectsByType`/`FindFirstObjectByType` apenas na inicialização ou carregamento de cena — nunca em `Update`/`FixedUpdate`.
- **Áudio e Objetos Temporários**:
  - `MusicManager.PlaySound` instancia um `GameObject` temporário com `AudioSource` e destrói após `clip.length`. Evite disparar `PlaySound` em frequência contínua/por frame.
  - `Sandevistan` faz `BakeMesh` + instanciação de `Mesh` e `Material` a cada `meshRefreshRate` durante o dash e limpa tudo no `OnDisable` para evitar leaks ao alternar modelos no `TimeTravel`. Ao editar efeitos similares, garanta destruição explícita de `Mesh` e `Material` criados em runtime.
- **Modelos 3D e Texturas**:
  - Modelos ficam em `Assets/Models/` (ou junto a prefabs específicos em `Assets/Prefab/`). Evite duplicar assets provisórios ou usar assets de `Assets/Models/_SemUso/` e `Assets/Materials/Tests_Materials/` em cenas finais.

---

## 5. Git e Fluxo de Trabalho

- **Regras do `.gitignore` e `.gitattributes`**:
  - `.gitignore` ignora `Library/`, `Temp/`, `Obj/`, `Build/`, `Builds/`, `Logs/`, `UserSettings/`, `.vs/`, `.vscode/`, `*.csproj`, `*.sln`, `*.slnx` e builds (`*.apk`, `*.aab`, `*.unitypackage`). `.gitattributes` define `* text=auto` (LF).
  - **Nunca** commitar pastas geradas (`Library/`, `Temp/`, `Logs/`, `.idea/`) nem arquivos `.csproj`/`.sln`.
- **Arquivos `.meta`**:
  - **Todo** asset, script ou pasta criada, movida ou removida em `Assets/` deve manter seu respectivo arquivo `.meta` sincronizado. Nunca delete/renomeie um `.cs`, `.prefab`, `.mat` ou pasta sem atualizar/remover o `.meta` correspondente — quebrar GUIDs desconecta referências em cenas e prefabs.
- **Cenas e Prefabs**:
  - Alterações em cenas principais (`Menu.unity`, `Lobby.unity`, `Level_Armazem.unity`, `museu.unity`) e prefabs compartilhados (`Canvas_HUD.prefab`, `Canvas_Info.prefab`, `PauseMenu.prefab`, `Boxes.prefab`) geram diffs YAML grandes e propensos a conflito. Faça mudanças mínimas, isoladas e prefira **Prefab Variants** ou novos scripts desacoplados antes de editar diretamente uma cena inteira.
  - Não altere arquivos em `ProjectSettings/` (especialmente `TagManager.asset`, `DynamicsManager.asset`, `EditorBuildSettings.asset`, `GraphicsSettings.asset`, `QualitySettings.asset`) sem necessidade explícita da tarefa.
  - Não toque em `Assets/Scenes/_Antigo/`, `Assets/_Recovery/` ou `Assets/Models/_SemUso/` sem instrução direta.
- **Boas práticas ao propor mudanças**:
  - Se alterar campos `[SerializeField]` ou assinaturas de métodos ligados no Inspector (especialmente callbacks de `InputSystem`, botões de UI ou `UnityEvent`s como `Interactable.inetractAction`), verifique e documente quais prefabs/cenas precisam ter a referência reatribuída.

---

## 6. Checklist Rápido Antes de Finalizar

Antes de concluir qualquer tarefa neste repositório:

1. **Compilação C# limpa**: verificar se não há erros ou warnings novos no Console do Unity / build de scripts.
2. **GUIDs e `.meta` íntegros**: confirmar que todo arquivo criado/movido/removido tem seu `.meta` coerente e não deixou referências `Missing (Mono Script)` ou `Missing Reference`.
3. **Impacto em Cenas e Prefabs**: se um `[SerializeField]` foi renomeado/adicionado ou um `InputAction` callback mudou de nome, checar se os prefabs (`Canvas_HUD`, `PauseMenu`, player, inimigos) e as cenas ativas do build continuam conectados.
4. **Ciclo de Vida e Eventos**: confirmar `RemoveListener` em `OnDestroy` para `TimeTravel.OnTimeChange` (e `SceneManager.sceneLoaded`), e cancelamento/limpeza de `LeanTween` e corrotinas em `OnDisable`/`OnDestroy`.
5. **Estado Global Restaurado**: garantir que nenhum caminho de erro/transição deixe `Time.timeScale == 0`, `Cursor.lockState` travado em UI ou `CharacterController.enabled == false` indevidamente.
6. **Git Status Limpo**: conferir `git status` para garantir que não há arquivos de `Backup`, cenas não relacionadas, `ProjectSettings` acidentais ou arquivos temporários incluídos no escopo.
7. **AGETNS.md atualizado**: Caso a mudanca deixe alguma mudanca no AGENTS.md desatualizada, atualiza-la para deixar o arquivo sempre um reflexo do projeto mais atualizado.

---

## 7. A Confirmar no Projeto

Os pontos abaixo foram observados no repositório, mas não ficam 100% claros apenas pelo código/configuração estática — confirme com o time antes de assumir comportamento padrão:

- **Input Actions ativo (`InputSystem_Actions.inputactions` vs. `PlayerActions.inputactions`)**: existem dois arquivos `.inputactions` na raiz de `Assets/`, e `EditorBuildSettings.asset` referencia o GUID de `InputSystem_Actions.inputactions` em `com.unity.input.settings.actions`, enquanto `PlayerActions.inputactions` define o action map `basic` (`Movement`, `ChangeTime`, `Dash`, `Slide`, `Snail Time`, etc.) usado pelos callbacks do player. Confirmar qual está vinculado ao `PlayerInput` nas cenas ativas antes de adicionar novas ações.
- **Persistência de progresso (`PlayerPrefs` / Save)**: apenas configurações de áudio (`Master`, `Music`, `SFX` em `SoundConfig.cs`) usam `PlayerPrefs`. A persistência de tutoriais em `TutorialTrigger.cs` está comentada, e `CollectibleManager` mantém estado apenas em memória estática durante a sessão. Confirmar se haverá sistema de save entre sessões.
- **Status das cenas `Level_Predio.unity`, `Level_Sky.unity` e `museu.unity`**: `Level_Predio` e `Level_Sky` estão desabilitadas (`enabled: 0`) em `EditorBuildSettings.asset`, enquanto `museu.unity` está habilitada (`enabled: 1`). Confirmar quais fases fazem parte do escopo atual de entregas.
- **Limpeza de assets legados e de teste**: `Assets/Prefab/UI/Backup/` (com alterações pendentes no working tree atual), `Assets/Scenes/_Antigo/`, `Assets/_Recovery/`, `Assets/Models/_SemUso/` e `Assets/Prefab/Samples/ProBuilder/` ocupam espaço no projeto; confirmar o que pode ser removido ou deve ser preservado.
- **Testes automatizados**: `com.unity.test-framework` (`1.6.0`) está instalado, porém não há pasta de testes ou `.asmdef` de testes do jogo em `Assets/`. Confirmar se existe convenção ou plano para EditMode/PlayMode tests.

---

## 8. Ferramentas disponíveis:

- **MCP: unityMCP**: Utilize o unity mcp para acessar informações de scene e outras informações extras no editor aberto da máquina.
- **Deploy de Subagents paralelos**: Caso haja alguma sub-tarefa dentro do objeto principal que possa ser feito de maneira pararela, delegue subagents para realizar a tarefa. 