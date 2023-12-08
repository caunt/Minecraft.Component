using Minecraft.Component.Component;
using SharpNBT;
using System;
using System.IO;

namespace Minecraft.Component.Serializer;

public static class NbtComponentSerializer
{
    public static Tag Serialize(ChatComponent component, string name = null)
    {
        if (component is StringComponent stringComponent)
            return new StringTag(name, stringComponent.Text);

        return SerializeCompoundTag(component);
    }

    public static ChatComponent Deserialize(Tag tag)
    {
        if (tag is StringTag stringTag)
            return new StringComponent(stringTag.Value);

        if (tag is CompoundTag compoundTag)
            return DeserializeCompoundTag(compoundTag);

        throw new NotSupportedException($"Deserializing {tag.Type} component not supported");
    }

    private static CompoundTag SerializeCompoundTag(ChatComponent component, string name = null)
    {
        var compoundTag = new CompoundTag(name);

        if (component is TranslationComponent translationComponent)
        {
            compoundTag.Add(new StringTag("translate", translationComponent.Translate));

            if (translationComponent.With.Count > 0)
                throw new NotSupportedException($"Translation NBT component \"with\" tag not supported (it is not possible to serialize CompoundTagArray)");
        }
        else if (component is StringComponent stringComponent)
        {
            compoundTag.Add(new StringTag("text", stringComponent.Text));
        }
        else if (component is ScoreComponent scoreComponent)
        {
            compoundTag.Add(new CompoundTag("score")
            {
                new StringTag("name", scoreComponent.Score.Name),
                new StringTag("objective", scoreComponent.Score.Objective),
                new StringTag("value", scoreComponent.Score.Value)
            });
        }
        else if (component is KeybindComponent keybindComponent)
        {
            compoundTag.Add(new StringTag("keybind", keybindComponent.Keybind));
        }

        if (component.Extra.Count > 0)
            throw new NotSupportedException($"{component} NBT component \"extra\" tag not supported (it is not possible to serialize CompoundTagArray)");

        return compoundTag;
    }
    
    private static ChatComponent DeserializeCompoundTag(CompoundTag compoundTag)
    {
        var keys = compoundTag.Keys;
        var type = typeof(ChatComponent);

        if (keys.Contains("translate"))
            type = typeof(TranslationComponent);
        else if (keys.Contains("text"))
            type = typeof(StringComponent);
        else if (keys.Contains("score"))
            type = typeof(ScoreComponent);
        else if (keys.Contains("keybind"))
            type = typeof(KeybindComponent);

        var instance = Activator.CreateInstance(type) as ChatComponent ?? throw new InvalidDataException($"Couldn't determine component type from {compoundTag.Stringify(false)}");

        if (instance is TranslationComponent translationComponent)
        {
            translationComponent.Translate = compoundTag.Get<StringTag>("translate").Value;

            if (keys.Contains("with"))
                throw new NotSupportedException($"Translation NBT component \"with\" tag not supported (it is not possible to deserialize CompoundTagArray)");
        }
        else if (instance is StringComponent stringComponent)
        {
            stringComponent.Text = compoundTag.Get<StringTag>("text").Value;
        }
        else if (instance is ScoreComponent scoreComponent)
        {
            var scoreTag = compoundTag.Get<CompoundTag>("score");

            scoreComponent.Score = new ScoreData
            {
                Name = scoreTag.Get<StringTag>("name").Value,
                Objective = scoreTag.Get<StringTag>("objective").Value,
                Value = scoreTag.Get<StringTag>("value").Value,
            };
        }
        else if (instance is KeybindComponent keybindComponent)
        {
            keybindComponent.Keybind = compoundTag.Get<StringTag>("keybind").Value;
        }

        if (keys.Contains("extra"))
            throw new NotSupportedException($"{type} NBT component \"extra\" tag not supported (it is not possible to deserialize CompoundTagArray)");

        return instance;
    }
}