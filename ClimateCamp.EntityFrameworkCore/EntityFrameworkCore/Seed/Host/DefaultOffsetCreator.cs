using ClimateCamp.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class DefaultOffsetCreator
    {


        private readonly CommonDbContext _context;
        public static List<Offset> InitialOffsets => GetInitialOffsets();
        public DefaultOffsetCreator(CommonDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateInitialOffsets();
        }

        private void CreateInitialOffsets()
        {
            foreach (var offset in InitialOffsets)
            {
                AddOffsetIfNotExists(offset);
            }
        }

        private void AddOffsetIfNotExists(Offset offset)
        {
            if (_context.Offsets.IgnoreQueryFilters().Any(t => t.Title == offset.Title))
            {
                return;
            }

            _context.Offsets.Add(offset);
            _context.SaveChanges();
        }

        private static List<Offset> GetInitialOffsets()
        {
            return new List<Offset>
            {
                new Offset("Sustainable sunflowers",
                "This project is about growing sunflowers in a sustainable way.",
                "Sustainable sunflowers.",
              "This project is about growing sunflowers in a sustainable way.",
              "../../assets/img/offset/flowers.png",
              "€55/ton CO2",
              "75 t CO2",
              DateTime.UtcNow,
              true),
                 new Offset("Green farming",
                "This project is about green farming in a sustainable way.",
                "Green farming.",
              "This project is about green farming in a sustainable way.",
              "../../assets/img/offset/green-farming.png",
              "€55/ton CO2",
              "75 t CO2",
              DateTime.UtcNow,
              true),
                 new Offset("DE plek",
                "This project is about 'De Plek' a sustainable farm.",
                "DE plek",
              "This project is about 'De Plek' a sustainable farm.",
              "../../assets/img/offset/De_plek.png",
              "€55/ton CO2",
              "75 t CO2",
              DateTime.UtcNow,
              true),
                 new Offset("Sustainable sunflowers",
                "This project is about growing sunflowers in a sustainable way.",
                "Sustainable sunflowers",
              "This project is about growing sunflowers in a sustainable way.",
              "../../assets/img/offset/sustainable-energy.png",
              "€55/ton CO2",
              "75 t CO2",
              DateTime.UtcNow,
              true),
                 new Offset("Zonnewind cvba",
                "Helping a transition towards green energy.",
                "Zonnewind cvba",
              "Helping a transition towards green energy.",
              "../../assets/img/offser/Zonnepanelen.png",
              "€55/ton CO2",
              "75 t CO2",
              DateTime.UtcNow,
              true),
                 new Offset("Sustainable sunflowers",
                "This project is about growing sunflowers in a sustainable way.",
                "Sustainable sunflowers.",
              "This project is about growing sunflowers in a sustainable way.",
              "../../assets/img/offset/offset/flowers.png",
              "€55/ton CO2",
              "75 t CO2",
              DateTime.UtcNow,
              true)
            };
        }
    }
}