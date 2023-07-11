import { Photo } from "./photo";

export interface LikedMemory {
    id: string;
    title: string;
    description: string;
    dateCreated: Date;
    photo: Photo;
}