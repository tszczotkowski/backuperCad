﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BackuperCad
{
	public partial class loadProfil : Form
	{
		String userName = Environment.UserName;
		String myDocument = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
		public loadProfil(StartWindow form1)
		{
			InitializeComponent();
			List<string> BackupList = new List<string>(Directory.GetDirectories(myDocument));
			foreach (string element in BackupList)
			{
				if (element.Contains("backuperCad_"))
				{
					selectedProfil.Items.Add(element);
				}
			}
		}

		private void b2_Click(object sender, EventArgs e)
		{
			progresMoment.ForeColor = Color.FromArgb(0, 0, 0);
			progresMoment.Text = " ";
			Refresh();
			String selectedProgram = sProgram.Text;
			string[] programElements = new string[2];
			programElements = OperationCAD.WhatCopy(selectedProgram);
			String program = programElements[0];
			String selectedProfileToBeRestored = selectedProfil.Text;
			//String reg = programElements[1];
			String sourcePath = selectedProfileToBeRestored;
			String roamingSource = selectedProfileToBeRestored + "\\Roaming";
			String localSource = selectedProfileToBeRestored + "\\Local";
			String pathReg = sourcePath + "\\regCopy.reg";
			String apdataRoamin = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString();
			String apdataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString();
			String targetRoaming = apdataRoamin + "\\" + program;
			String targetLocal = apdataLocal + "\\" + program;

			//MessageBox.Show("sRoaming: " + roamingSource +"\n" + "sSource: "  + localSource + "\n" +"targetRoaming: " + targetRoaming + "\n" + "targetLocal: " + targetLocal + "\n" + pathReg) ;
			if (!String.IsNullOrEmpty(selectedProfileToBeRestored) && !String.IsNullOrEmpty(program) && selectedProfileToBeRestored.Contains(program))
			{
				progresMoment.ForeColor = Color.FromArgb(0, 0, 0);
				progresMoment.Text = "Usuwam obecny profil CAD...";
				Refresh();
				if (Directory.Exists(targetRoaming) && Directory.Exists(targetLocal))
				{
					try
					{
						Directory.Delete(targetRoaming, true);
						Directory.Delete(targetLocal, true);
					}
					catch
					{
						
						progresMoment.ForeColor = Color.FromArgb(255, 0, 0);
						progresMoment.Text = "Wystąpił błąd poczas czyszczenie obecnego profilu " + program;
						Refresh();
					}
					

				}

				progresMoment.ForeColor = Color.FromArgb(0, 0, 0);
				progresMoment.Text = "Wgrywam nowy profil CAD...";
				Refresh();
				try
				{
					OperationCAD.DirectoryCopy(roamingSource, targetRoaming, true);
					OperationCAD.DirectoryCopy(localSource, targetLocal, true);
				}
				catch
				{
					progresMoment.ForeColor = Color.FromArgb(255, 0, 0);
					progresMoment.Text = "Wystąpił błąd podczas kopiowania plików " + program;
					Refresh();
					

				}
				progresMoment.ForeColor = Color.FromArgb(0, 0, 0);
				progresMoment.Text = "Importuje rejestr...";
				Refresh();
				try
				{
					OperationCAD.importRegistry(pathReg);
				}
				catch
				{
					
					progresMoment.ForeColor = Color.FromArgb(255, 0, 0);
					progresMoment.Text = "Wystapił błąd podczas imporotwania rejestru" ;
					Refresh();
				}

				progresMoment.ForeColor = Color.FromArgb(0, 255, 0);
				progresMoment.Text = "Skończone";
				Refresh();
			}
			else
			{
				progresMoment.ForeColor = Color.FromArgb(255, 0, 0);
				progresMoment.Text = "Sprawdź czy wszystkie pola zostały uzupełnione poprawnie";
				Refresh();
				
			}

		}
	}
}
