﻿/*
------------------------------------------------
Generated by Cradle 2.0.2.0
https://github.com/daterre/Cradle

Original file: Lv1_Dialogue.twee
Story format: Sugar
------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cradle;
using IStoryThread = System.Collections.Generic.IEnumerable<Cradle.StoryOutput>;
using Cradle.StoryFormats.Sugar;

public partial class @Lv1_Dialogue: Cradle.StoryFormats.Sugar.SugarStory
{
	#region Variables
	// ---------------

	public class VarDefs: RuntimeVars
	{
		public VarDefs()
		{
			VarDef("crouchKey", () => this.@crouchKey, val => this.@crouchKey = val);
		}

		public StoryVar @crouchKey;
	}

	public new VarDefs Vars
	{
		get { return (VarDefs) base.Vars; }
	}

	// ---------------
	#endregion

	#region Initialization
	// ---------------


	@Lv1_Dialogue()
	{
		this.StartPassage = "Start";

		base.Vars = new VarDefs() { Story = this, StrictMode = false };


		base.Init();
		passage0_Init();
		passage1_Init();
		passage2_Init();
		passage3_Init();
		passage4_Init();
		passage5_Init();
		passage6_Init();
		passage7_Init();
		passage8_Init();
		passage9_Init();
		passage10_Init();
		passage11_Init();
		passage12_Init();
		passage13_Init();
		passage14_Init();
		passage15_Init();
		passage16_Init();
		passage17_Init();
		passage18_Init();
		passage19_Init();
		passage20_Init();
		passage21_Init();
	}

	// ---------------
	#endregion

	// .............
	// #0: Naranjito

	void passage0_Init()
	{
		this.Passages[@"Naranjito"] = new StoryPassage(@"Naranjito", new string[]{  }, passage0_Main);
	}

	IStoryThread passage0_Main()
	{
		if (! visited("Introduccion"))
		{
			yield return lineBreak();
			yield return text(@"    Naranjito: ¿De dónde has salido tú...?");
			yield return lineBreak();
			yield return text(@"    Naranjito: Da igual, estoy demasiado cansado para pensar");
			yield return lineBreak();
			yield return text(@"    Naranjito: Deberías volver a la ciudad antes de que te vean los helados");
			yield return lineBreak();
			yield return text(@"    Naranjito: No permiten que nadie merodee por aquí fuera de su turno");
			yield return lineBreak();
		}
		yield return lineBreak();
		yield return text(@"Naranjito: <i>Cúbrete detrás de los escombros</i>");
		yield return lineBreak();
		yield return text(@"Naranjito: <i>y usa ""{");
		yield return text(Vars.crouchKey);
		yield return text(@"}"" para que sea más difícil que te detecten</i>...");
		yield return lineBreak();
		yield return text(@"Naranjito: Pero que demonios he dicho, esa frase no ha salido de mí");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #1: Guardia1

	void passage1_Init()
	{
		this.Passages[@"Guardia1"] = new StoryPassage(@"Guardia1", new string[]{  }, passage1_Main);
	}

	IStoryThread passage1_Main()
	{
		yield return text(@"Guardia: VAMOS VAAAMOS!!! Todo ese hielo no se va a picar solo");
		yield return lineBreak();
		yield return text(@"Guardia: Polos número 234 y 56 no me gustaría llevaros a la zona de.. <i><b>reformación</b></i>");
		yield return lineBreak();
		yield return text(@"Guardia: HAHAHAHA.");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #2: Chups

	void passage2_Init()
	{
		this.Passages[@"Chups"] = new StoryPassage(@"Chups", new string[]{  }, passage2_Main);
	}

	IStoryThread passage2_Main()
	{
		if (! visited("Chups"))
		{
			yield return lineBreak();
			yield return text(@"    Chups: ¡Muchas gracias!, ya me estaba empezando a derretir de tanto picar");
			yield return lineBreak();
		}
		yield return lineBreak();
		yield return text(@"Chups: Me pregunto qué harán con todo ese hielo, nunca tienen suficiente");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #3: Helen

	void passage3_Init()
	{
		this.Passages[@"Helen"] = new StoryPassage(@"Helen", new string[]{  }, passage3_Main);
	}

	IStoryThread passage3_Main()
	{
		if (! visited("Helen"))
		{
			yield return lineBreak();
			yield return text(@"    Helen: Creía que no se iba a acabar nunca, te debo una");
			yield return lineBreak();
		}
		yield return lineBreak();
		yield return text(@"Helen: Cada vez los turnos son más largos");
		yield return lineBreak();
		yield return text(@"Helen: Con este ritmo voy a acabar en la zona de reformación");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #4: Huranyo

	void passage4_Init()
	{
		this.Passages[@"Huranyo"] = new StoryPassage(@"Huranyo", new string[]{  }, passage4_Main);
	}

	IStoryThread passage4_Main()
	{
		if (! visited("Huranyo"))
		{
			yield return lineBreak();
			yield return text(@"    Prefiero que no nos vean hablar");
			yield return lineBreak();
		}
		else
		{
			yield return lineBreak();
			yield return text(@"    (...)");
			yield return lineBreak();
		}
		yield return lineBreak();
		yield break;
	}


	// .............
	// #5: Palanquilla

	void passage5_Init()
	{
		this.Passages[@"Palanquilla"] = new StoryPassage(@"Palanquilla", new string[]{  }, passage5_Main);
	}

	IStoryThread passage5_Main()
	{
		if (has_item("lever"))
		{
			yield return lineBreak();
			yield return text(@"    ");
			if (! visited("Palanquilla"))
			{
				yield return lineBreak();
				yield return text(@"        Palanquilla: Madre mía... he perdido la manivela y ahora no se puede usar el <b>ascensor</b>");
				yield return lineBreak();
				yield return text(@"    ");
			}
			yield return lineBreak();
			yield return text(@"    ");
			yield return link(@" Palanquilla.Ascensor ", @" Palanquilla.Ascensor ", null);
			yield return lineBreak();
		}
		yield return lineBreak();
		yield return text(@"Palanquilla: Madre mía... he perdido la manivela y ahora no se puede usar el <b>ascensor</b>");
		yield return lineBreak();
		yield return text(@"Palanquilla: Como se enteren los guardias me van a dejar al Sol");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #6: Olafs

	void passage6_Init()
	{
		this.Passages[@"Olafs"] = new StoryPassage(@"Olafs", new string[]{  }, passage6_Main);
	}

	IStoryThread passage6_Main()
	{
		if (! visited("Olafs"))
		{
			yield return lineBreak();
			yield return text(@"    Olaf: No está permitido pasar a la ciudad hasta que no reúnas <b>100</b> hielos");
			yield return lineBreak();
		}
		yield return lineBreak();
		if (quantity_item("ice") < 100)
		{
			yield return lineBreak();
			yield return text(@"    Twolaf: Aún no tienes suficientes... HAHAHAHAH");
			yield return lineBreak();
		}
		else
		{
			yield return lineBreak();
			yield return text(@"    ");
			yield return link(@" Olafs.Hielos ", @" Olafs.Hielos ", null);
			yield return lineBreak();
		}
		yield return lineBreak();
		yield break;
	}


	// .............
	// #7: Alfredo

	void passage7_Init()
	{
		this.Passages[@"Alfredo"] = new StoryPassage(@"Alfredo", new string[]{  }, passage7_Main);
	}

	IStoryThread passage7_Main()
	{
		if (! visited("Alfredo"))
		{
			yield return lineBreak();
			yield return text(@"    Alfredo: Desde que el alcalde <b>Mc Topping</b> mandó encender ese trasto...");
			yield return lineBreak();
			yield return text(@"	Alfredo: se están produciendo muchos derrumbamientos");
			yield return lineBreak();
			yield return text(@"    Alfredo: Además está acabando con todo el hielo y cada vez hace más calor");
			yield return lineBreak();
		}
		yield return lineBreak();
		if (done_mission("rollball"))
		{
			yield return lineBreak();
			yield return text(@"    ");
			yield return link(@" Alfredo.Motor ", @" Alfredo.Motor ", null);
			yield return lineBreak();
		}
		yield return lineBreak();
		yield return text(@"Alfredo: Ojalá alguien lo apague antes de que la situación sea irreversible");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #8: Alfredo.Motor

	void passage8_Init()
	{
		this.Passages[@"Alfredo.Motor"] = new StoryPassage(@"Alfredo.Motor", new string[]{  }, passage8_Main);
	}

	IStoryThread passage8_Main()
	{
		if (! visited("Alfredo.Motor"))
		{
			yield return lineBreak();
			yield return text(@"    Alfredo: Woow! Cómo han corrido esos helados");
			yield return lineBreak();
		}
		else
		{
			yield return lineBreak();
			yield return text(@"    Alfredo: La ciudad entera fue construida sobre un casquete de hielo");
			yield return lineBreak();
			yield return text(@"    Alfredo: No podemos seguir explotando los recursos a esta velocidad");
			yield return lineBreak();
			yield return text(@"    Alfredo: ¡Tenemos que restaurar la cadena del frío!");
			yield return lineBreak();
		}
		yield return lineBreak();
		yield break;
	}


	// .............
	// #9: Palanquilla.Ascensor

	void passage9_Init()
	{
		this.Passages[@"Palanquilla.Ascensor"] = new StoryPassage(@"Palanquilla.Ascensor", new string[]{  }, passage9_Main);
	}

	IStoryThread passage9_Main()
	{
		if (! visited("Palanquilla.Ascensor"))
		{
			yield return lineBreak();
			yield return text(@"    Palanquilla: Me has salvado la vida... ahora mismo pongo en marcha este trasto");
			yield return lineBreak();
		}
		else
		{
			yield return lineBreak();
			yield return text(@"	Palanquilla: Mira como gira, el alcalde estaría orgulloso de mí");
			yield return lineBreak();
		}
		yield return lineBreak();
		yield break;
	}


	// .............
	// #10: Olafs.Hielos

	void passage10_Init()
	{
		this.Passages[@"Olafs.Hielos"] = new StoryPassage(@"Olafs.Hielos", new string[]{  }, passage10_Main);
	}

	IStoryThread passage10_Main()
	{
		if (! visited("Olafs.Hielos"))
		{
			yield return lineBreak();
			yield return text(@"    Olaf: ¿Có..Cómo?.. ¿Qué ya lo tienes? Está bien... pasa...");
			yield return lineBreak();
		}
		else
		{
			yield return lineBreak();
			yield return text(@"    Twolaf: ¿Qué quieres ahora polito?");
			yield return lineBreak();
		}
		yield return lineBreak();
		yield break;
	}


	// .............
	// #11: Guardia2y3

	void passage11_Init()
	{
		this.Passages[@"Guardia2y3"] = new StoryPassage(@"Guardia2y3", new string[]{  }, passage11_Main);
	}

	IStoryThread passage11_Main()
	{
		yield return text(@"Guardia: Hemos conseguido un buen montón de hielos HAHAHAHAH");
		yield return lineBreak();
		yield return text(@"Guardia: Seguro que el jefe nos da una buena comisión");
		yield return lineBreak();
		yield return text(@"Guardia: Siempre podemos quedarnos un poco más, y echarles la culpa a esos estúpidos polos");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #12: Ramon

	void passage12_Init()
	{
		this.Passages[@"Ramon"] = new StoryPassage(@"Ramon", new string[]{  }, passage12_Main);
	}

	IStoryThread passage12_Main()
	{
		if (! visited("Ramon"))
		{
			yield return lineBreak();
			yield return text(@"    Ramon: Estoy muy cansado...");
			yield return lineBreak();
		}
		else
		{
			yield return lineBreak();
			yield return text(@"    Ramon: (...)");
			yield return lineBreak();
		}
		yield return lineBreak();
		yield break;
	}


	// .............
	// #13: Mentolado

	void passage13_Init()
	{
		this.Passages[@"Mentolado"] = new StoryPassage(@"Mentolado", new string[]{  }, passage13_Main);
	}

	IStoryThread passage13_Main()
	{
		yield return text(@"Mentolado: Está batidora es capaz de extraer tanto hielo como yo en un mes..");
		yield return lineBreak();
		yield return text(@"Mentolado: Los helados nos han amenazado con que, si no aumentamos la producción...");
		yield return lineBreak();
		yield return text(@"Mentolado: vamos a acabar en la <b>zona de reformación</b>");
		yield return lineBreak();
		yield return text(@"Mentolado: No quiero ir ahí... por eso estoy aquí escondido");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #14: Bombon

	void passage14_Init()
	{
		this.Passages[@"Bombon"] = new StoryPassage(@"Bombon", new string[]{  }, passage14_Main);
	}

	IStoryThread passage14_Main()
	{
		if (! visited("Bombon"))
		{
			yield return lineBreak();
			yield return text(@"    Bombón: ¡Por fin alguien de hielo y palo por aquí!");
			yield return lineBreak();
			yield return text(@"    Bombón: Cuando empezó esa batidora a funcionar se derrumbó el ascensor...");
			yield return lineBreak();
			yield return text(@"	Bombón: y me quedé aquí atrapado");
			yield return lineBreak();
			yield return text(@"    Bombón: Ahora me dedico a hacer bolas de nieve ¿Quieres probar?");
			yield return lineBreak();
		}
		yield return lineBreak();
		if (done_mission("rollball"))
		{
			yield return lineBreak();
			yield return text(@"    ");
			yield return link(@" Bombon.Bola ", @" Bombon.Bola ", null);
			yield return text(@" ");
			yield return lineBreak();
		}
		yield return text(@" ");
		yield return lineBreak();
		yield return text(@"Bombón: Saca la bola de nieve y arrástrala por la superficie para hacerla más grande");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #15: Bombon.Bola

	void passage15_Init()
	{
		this.Passages[@"Bombon.Bola"] = new StoryPassage(@"Bombon.Bola", new string[]{  }, passage15_Main);
	}

	IStoryThread passage15_Main()
	{
		if (! visited("Bombon.Bola"))
		{
			yield return lineBreak();
			yield return text(@"    Bombón: ¡Nunca había visto una bola tan grade!");
			yield return lineBreak();
			yield return text(@"	Bombón: pero lo siento no te puedo dar más, eres un peligro para la sociedad");
			yield return lineBreak();
		}
		else
		{
			yield return lineBreak();
			yield return text(@"    Bombón: Lo siento no te puedo dar más, eres un peligro para la sociedad");
			yield return lineBreak();
		}
		yield return lineBreak();
		yield break;
	}


	// .............
	// #16: Arrestado

	void passage16_Init()
	{
		this.Passages[@"Arrestado"] = new StoryPassage(@"Arrestado", new string[]{  }, passage16_Main);
	}

	IStoryThread passage16_Main()
	{
		if (random_bool())
		{
			yield return lineBreak();
			yield return text(@"	++Arrestado: ¡¡NO DEBERÍAS ESTAR AQUÍ, QUEDAS ARRESTADO!!");
			yield return lineBreak();
		}
		else if (random_bool())
		{
			yield return lineBreak();
			yield return text(@"	++Arrestado: SE TE ACABO EL JUEGO!!");
			yield return lineBreak();
		}
		else
		{
			yield return lineBreak();
			yield return text(@"	++Arrestado: DIRECTO A LA ZONA DE REFORMACIÓN!!");
			yield return lineBreak();
		}
		yield return lineBreak();
		yield break;
	}


	// .............
	// #17: Auriculares

	void passage17_Init()
	{
		this.Passages[@"Auriculares"] = new StoryPassage(@"Auriculares", new string[]{  }, passage17_Main);
	}

	IStoryThread passage17_Main()
	{
		yield return text(@"Auriculares: A TODOS LOS CIUDADANOS, LES HABLA EL ALCALDE <b>MC TOPPING</b>");
		yield return lineBreak();
		yield return text(@"Auriculares: SIGAN TRABAJANDO DURO, LA NEVERA NO PERDONA LA DEBILIDAD");
		yield return lineBreak();
		yield return text(@"Auriculares: JUNTOS VAMOS A CONSTRUIR UN FUTURO GÉLIDO");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #18: Alfredo.Bola

	void passage18_Init()
	{
		this.Passages[@"Alfredo.Bola"] = new StoryPassage(@"Alfredo.Bola", new string[]{  }, passage18_Main);
	}

	IStoryThread passage18_Main()
	{
		yield return text(@"Alfredo: Carámbanos!");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #19: SinPico

	void passage19_Init()
	{
		this.Passages[@"SinPico"] = new StoryPassage(@"SinPico", new string[]{  }, passage19_Main);
	}

	IStoryThread passage19_Main()
	{
		yield return text(@"Pol: (Me hace falta el pico para esto)");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #20: SinCuchara

	void passage20_Init()
	{
		this.Passages[@"SinCuchara"] = new StoryPassage(@"SinCuchara", new string[]{  }, passage20_Main);
	}

	IStoryThread passage20_Main()
	{
		yield return text(@"Pol: (Creo que necesito una cuchara aquí)");
		yield return lineBreak();
		yield break;
	}


	// .............
	// #21: SinBola

	void passage21_Init()
	{
		this.Passages[@"SinBola"] = new StoryPassage(@"SinBola", new string[]{  }, passage21_Main);
	}

	IStoryThread passage21_Main()
	{
		yield return text(@"Pol: (Debería ayudar a ese polo antes)");
		yield return lineBreak();
		yield break;
	}


}