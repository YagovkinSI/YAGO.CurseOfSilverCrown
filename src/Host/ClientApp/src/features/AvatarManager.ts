import type { Gender, Race } from "../entities/Character";

interface Avatar {
  name: string,
  priority: number
}

const avatars: Avatar[] = [
  { name: 'Daji_Female_1', priority: 2 },
  { name: 'Daji_Female_2', priority: 3 },
  { name: 'Daji_Male_1', priority: 1 },
  { name: 'Daji_Male_2', priority: 3 },

  { name: 'Elnir_Female_1', priority: 3 },
  { name: 'Elnir_Female_2', priority: 1 },
  { name: 'Elnir_Female_3', priority: 3 },
  { name: 'Elnir_Male_1', priority: 2 },
  { name: 'Elnir_Male_2', priority: 3 },

  { name: 'Isian_Female_1', priority: 2 },
  { name: 'Isian_Female_2', priority: 3 },
  { name: 'Isian_Female_3', priority: 3 },
  { name: 'Isian_Female_4', priority: 3 },
  { name: 'Isian_Male_1', priority: 1 },
  { name: 'Isian_Male_2', priority: 3 },
  { name: 'Isian_Male_3', priority: 3 },

  { name: 'Khashin_Female_1', priority: 2 },
  { name: 'Khashin_Female_2', priority: 3 },
  { name: 'Khashin_Male_1', priority: 3 },
  { name: 'Khashin_Male_2', priority: 1 },

  { name: 'Khazadin_Female_1', priority: 3 },
  { name: 'Khazadin_Female_2', priority: 2 },
  { name: 'Khazadin_Female_3', priority: 3 },
  { name: 'Khazadin_Male_1', priority: 1 },
  { name: 'Khazadin_Male_2', priority: 3 },
  { name: 'Khazadin_Male_3', priority: 3 },

  { name: 'Nahumi_Female_1', priority: 3 },
  { name: 'Nahumi_Female_2', priority: 2 },
  { name: 'Nahumi_Female_3', priority: 3 },
  { name: 'Nahumi_Male_1', priority: 1 },
  { name: 'Nahumi_Male_2', priority: 3 },
];

const getMatchingAvatars = (race: Race, gender: Gender, num?: number): string[] => {
  return avatars
    .filter(avatar => {
      const [avatarRace, avatarGender, avatarNum] = avatar.name.split('_');
      const raceMatches = race === 'Unknown' || avatarRace === race;
      const genderMatches = gender === 'Unknown' || avatarGender === gender;
      const numMatches = num === undefined || avatarNum === num.toString();
      return raceMatches && genderMatches && numMatches;
    })
    .sort((a, b) => a.priority - b.priority)
    .map(item => item.name);
}

export const getAvatar = (race: Race, gender: Gender, num?: number): string => {
  const matchingAvatars = getMatchingAvatars(race, gender, num);
  return matchingAvatars[0];
}

export const getAvatarCount = (race: Race, gender: Gender): number => {
  return getMatchingAvatars(race, gender).length;
}
