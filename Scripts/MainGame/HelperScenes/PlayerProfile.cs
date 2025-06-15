using EsportsManager.EsportsManager.Data.Models;
using EsportsManager.Scripts.MainMenu.Factories;
using Godot;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public partial class PlayerProfile : Control
{
	[Export] private Label Rating;
	[Export] private TextureRect Flag;
	[Export] private Label Nickname;
	[Export] private Label Position;
	[Export] private Label Age;

	[Export] private Label Label1;
	[Export] private Label rating1;
	[Export] private Label Label2;
	[Export] private Label rating2;
	[Export] private Label Label3;
	[Export] private Label rating3;
	[Export] private Label Label4;
	[Export] private Label rating4;
	[Export] private Label Label5;
	[Export] private Label rating5;
	[Export] private Label Label6;
	[Export] private Label rating6;

	GameState gameState;
	GameDbContext context;
	Player player;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameState = GameService.TransferGameState();
		context = GameService.LoadDataBaseInformation();
		MouseFilter = Control.MouseFilterEnum.Pass;

		MouseEntered += () => Modulate = new Color(1, 1, 1, 0.8f); // Example: fade on hover
		MouseExited += () => Modulate = new Color(1, 1, 1, 1);     // Reset on exit
		UpdateUI();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetPlayer(Player player)
	{
		this.player = player;
		UpdateUI();
	}

	private void UpdateUI()
	{
		Nickname.Text = player.Nickname;
		Rating.Text = player.Rating.ToString();
		Rating.AddThemeColorOverride("font_color", DetermineRatingColor(player.Rating));
		DeterminePlayerStats();
		Flag.Texture = ResourceLoader.Load<Texture2D>(DeterminePlayerNationality(player));
		Age.Text = player.Age.ToString();
	}
	private Color DetermineRatingColor(double rating)
	{
		if(rating >= 100) return Colors.Aqua;
		else if(rating >= 85 && rating <=99) return new Color(0f, 0.39f, 0f);
		else if(rating >= 70 && rating <85) return new Color(0.56f, 0.93f, 0.56f);
		else if(rating >= 50 && rating <70) return new Color(1f, 0.84f, 0f);
		else if(rating >= 40 && rating <50) return new Color(1f, 0.65f, 0f);
		else if(rating >= 20 && rating <40) return new Color(1f, 0.5f, 0.5f);
		else return new Color(0.55f, 0f, 0f);
	}
	private void DeterminePlayerStats()
	{
		context = GameService.LoadDataBaseInformation();
		var csProfile = context.GameProfiles
		.OfType<GameProfile_CS>()  // Filter to only CS profiles
		.FirstOrDefault(gp => gp.PlayerId == player.Id);

		if (csProfile != null)
		{
			switch (csProfile.Role)
			{
				case Role_CS.Rifler:
					Label1.Text = "MEC";
					rating1.Text = Math.Floor(csProfile.Mechanics).ToString();
					Label2.Text = "REF";
					rating2.Text = Math.Floor(csProfile.Reflexes).ToString();
					Label3.Text = "ADA";
					rating3.Text = Math.Floor(csProfile.Adaptability).ToString();
					Label4.Text = "TEA";
					rating4.Text = Math.Floor(csProfile.Teamwork).ToString();
					Label5.Text = "COM";
					rating5.Text = Math.Floor(csProfile.Communication).ToString();
					Label6.Text = "POT";
					rating6.Text = Math.Floor(csProfile.Potential).ToString();
					Position.Text = Role_CS.Rifler.ToString();
					break;
				case Role_CS.AWPer:
					Label1.Text = "MEC";
					rating1.Text = Math.Floor(csProfile.Mechanics).ToString();
					Label2.Text = "REF";
					rating2.Text = Math.Floor(csProfile.Reflexes).ToString();
					Label3.Text = "CON";
					rating3.Text = Math.Floor(csProfile.Consistency).ToString();
					Label4.Text = "TAC";
					rating4.Text = Math.Floor(csProfile.Tactics).ToString();
					Label5.Text = "COM";
					rating5.Text = Math.Floor(csProfile.Communication).ToString();
					Label6.Text = "POT";
					rating6.Text = Math.Floor(csProfile.Potential).ToString();
					Position.Text = Role_CS.AWPer.ToString();
					break;
				case Role_CS.InGameLeader:
					Label1.Text = "GAM";
					rating1.Text = Math.Floor(csProfile.GameSense).ToString();
					Label2.Text = "COM";
					rating2.Text = Math.Floor(csProfile.Communication).ToString();
					Label3.Text = "ADA";
					rating3.Text = Math.Floor(csProfile.Adaptability).ToString();
					Label4.Text = "TAC";
					rating4.Text = Math.Floor(csProfile.Tactics).ToString();
					Label5.Text = "CON";
					rating5.Text = Math.Floor(csProfile.Consistency).ToString();
					Label6.Text = "POT";
					rating6.Text = Math.Floor(csProfile.Potential).ToString();
					Position.Text = "IGL";
					break;
				case Role_CS.Lurker:
					Label1.Text = "GAM";
					rating1.Text = Math.Floor(csProfile.GameSense).ToString();
					Label2.Text = "MEC";
					rating2.Text = Math.Floor(csProfile.Mechanics).ToString();
					Label3.Text = "ADA";
					rating3.Text = Math.Floor(csProfile.Adaptability).ToString();
					Label4.Text = "TAC";
					rating4.Text = Math.Floor(csProfile.Tactics).ToString();
					Label5.Text = "CON";
					rating5.Text = Math.Floor(csProfile.Consistency).ToString();
					Label6.Text = "POT";
					rating6.Text = Math.Floor(csProfile.Potential).ToString();
					Position.Text = Role_CS.Lurker.ToString();
					break;
				case Role_CS.Support:
					Label1.Text = "GAM";
					rating1.Text = Math.Floor(csProfile.GameSense).ToString();
					Label2.Text = "ADA";
					rating2.Text = Math.Floor(csProfile.Adaptability).ToString();
					Label3.Text = "COM";
					rating3.Text = Math.Floor(csProfile.Communication).ToString();
					Label4.Text = "TAC";
					rating4.Text = Math.Floor(csProfile.Tactics).ToString();
					Label5.Text = "CON";
					rating5.Text = Math.Floor(csProfile.Consistency).ToString();
					Label6.Text = "POT";
					rating6.Text = Math.Floor(csProfile.Potential).ToString();
					Position.Text = Role_CS.Support.ToString();
					break;
			}
		}
	}

	private string DeterminePlayerNationality(Player player)
	{
		var imageName = player.Nationality.ToLower() switch
		{
			"usa" => "us.png",
			"france" => "fr.png",
			"germany" => "de.png",
			"bulgaria" => "bg.png",
			"kazakhstan" => "kz.png",
			"serbia" => "rs.png",
			"sweden" => "se.png",
			"norway" => "no.png",
			"denmark" => "dk.png",
			"finland" => "fi.png",
			"poland" => "pl.png",
			"russia" => "ru.png",
			"south korea" => "kr.png",
			"canada" => "ca.png",
			"mexico" => "mx.png",
			"argentina" => "ar.png",
			"brazil" => "br.png",
			"greece" => "gr.png",
			"turkey" => "tr.png",
			"ukraine" => "ua.png",
			"romania" => "ro.png",
			"colombia" => "co.png",
			"india" => "in.png",
			"australia" => "au.png",
			"south africa" => "za.png",
			"china" => "cn.png",
			"japan" => "jp.png",
			"uk" => "gb.png",
			"spain" => "es.png",
			"netherlands" => "nl.png",
			"italy" => "it.png",
			"albania" => "al.png",
			"kosovo" => "xk.png",
			"mongolia" => "mn.png",
			"indonesia" => "id.png",
			"vietnam" => "vn.png",
			"philippines" => "ph.png",
			"am" => "a_m.png",
			"eu" => "e_u.png",
			"as" => "a_s.png",
			_ => "flag_placeholder.png"
		};
		return $"Images/CountryFlags/{imageName}";
	}
}
