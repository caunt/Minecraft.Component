using Minecraft.Component.Component;
using SharpNBT;
using System;
using System.IO;

namespace Minecraft.Component.Serializer;
public static class NbtComponentSerializer
{
    public static ChatComponent Deserialize(Tag tag)
    {
        if (tag is StringTag stringTag)
            return new StringComponent(stringTag.Value);

        if (tag is CompoundTag compoundTag)
            return DecodeCompoundTag(compoundTag);

        throw new NotSupportedException($"Deserializing {tag.Type} component not supported");
    }

    private static ChatComponent DecodeCompoundTag(CompoundTag compoundTag)
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
                throw new NotSupportedException($"Translation NBT component \"with\" tag not supported (it is not possible to serialize CompoundTagArray)");
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
            throw new NotSupportedException($"{type} NBT component \"extra\" tag not supported (it is not possible to serialize CompoundTagArray)");

        return instance;
    }
}