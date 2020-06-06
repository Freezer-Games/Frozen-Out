namespace Scripts.Level.NPC
{
    public class CortesInfo : NPCInfo
    {
        private readonly string[] GiroIzquierdaTriggers = new string[]
        {
            "Anim_Giro_Izq"
        };
        private readonly string[] GiroDerechaTriggers = new string[]
        {
            "Anim_Giro_Der"
        };
        private readonly string[] DenyTriggers = new string[]
        {
            "Anim_Negacion"
        };
        private readonly string[] InclinadoEntradaTriggers = new string[]
        {
            "Anim_Inclinado_Entrada"
        };
        private readonly string[] InclinadoSalidaTriggers = new string[]
        {
            "Anim_Inclinado_Salida"
        };

        public override void StartAnimation(string animation)
        {
            switch (animation)
            {
                case "Giro_Izq":
                    SetRandomTrigger(GiroIzquierdaTriggers);
                    break;
                case "Giro_Der":
                    SetRandomTrigger(GiroDerechaTriggers);
                    break;
                case "Deny":
                    SetRandomTrigger(DenyTriggers);
                    break;
                case "Inclinado_Entrada":
                    SetRandomTrigger(InclinadoEntradaTriggers);
                    break;
                case "Inclinado_Salida":
                    SetRandomBool(InclinadoSalidaTriggers);
                    break;
                default:
                    break;
            }
        }
    }
}