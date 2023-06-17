using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Memphis = ProjectMemphis.Data;

namespace ProjectMemphis.Bot.Data
{
    public static class BotFrameworkExtensionMethods
    {
        public static Attachment ToAttachment(this Memphis.Event stateEvent, Memphis.Node node, ulong step)
        {
            if (stateEvent == null)
            {
                throw new ArgumentNullException(nameof(stateEvent));
            }

            var optionsEvent = stateEvent as Memphis.OptionsEvent;
            if (optionsEvent != null)
            {
                return optionsEvent.ToAttachment(node, step);
            }

            var pictureEvent = stateEvent as Memphis.PictureEvent;
            if (pictureEvent != null)
            {
                return pictureEvent.ToAttachment(node, step);
            }

            throw new NotImplementedException(nameof(stateEvent.GetType));
        }

        public static Attachment ToAttachment(this Memphis.OptionsEvent optionsEvent, Memphis.Node node, ulong step)
        {
            if (optionsEvent.Options == null || optionsEvent.Options.Count == 0)
            {
                throw new ArgumentException(nameof(optionsEvent.Options));
            }

            var actions = optionsEvent.Options.Select(option => option.ToCardAction(node, step)).ToList();
            var heroCard = new HeroCard { Title = optionsEvent.Title, Buttons = actions };
            return heroCard.ToAttachment();
        }

        public static CardAction ToCardAction(this Memphis.Option option, Memphis.Node node, ulong step)
        {
            var stateOption = option as Memphis.StateOption;
            if (stateOption != null)
            {
                return stateOption.ToCardAction(node, step);
            }

            var urlOption = option as Memphis.UrlOption;
            if (urlOption != null)
            {
                return urlOption.ToCardAction(node, step);
            }

            throw new NotImplementedException(nameof(option.GetType));
        }

        public static CardAction ToCardAction(this Memphis.StateOption option, Memphis.Node node, ulong step)
        {
            return new CardAction
            {
                Title = option.Text,
                Type = "postBack",
                Value = new Memphis.Transition { SourceNode = node, TargetNode = option.TargetNode, Step = step }.Serialize()
            };
        }

        public static CardAction ToCardAction(this Memphis.UrlOption option, Memphis.Node node, ulong step)
        {
            return new CardAction
            {
                Title = option.Text,
                Type = "openUrl",
                Value = option.Url
            };
        }

        public static Attachment ToAttachment(this Memphis.PictureEvent pictureEvent, Memphis.Node node, ulong step)
        {
            var imageCard = new CardImage { Url = pictureEvent.ImageUrl, Alt = pictureEvent.Alt };
            if (pictureEvent.TargetNode != null)
            {
                imageCard.Tap = new CardAction { Value = new Memphis.Transition { SourceNode = node, TargetNode = pictureEvent.TargetNode, Step = step }.Serialize() };
            }

            var heroCard = new HeroCard { Images = new List<CardImage> { imageCard } };
            return heroCard.ToAttachment();
        }

        public static string Serialize(this Memphis.Transition transtition)
        {
            try
            {
                return JsonConvert.SerializeObject(transtition);
            }
            catch (Exception)
            {
                return default(string);
            }
        }
    }
}